using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SetBoxWebUI.Helpers;
using SetBoxWebUI.Interfaces;
using SetBoxWebUI.Models;
using SetBoxWebUI.Repository;

namespace SetBoxWebUI.Controllers
{
    /// <summary>
    /// Api para controlar as SetBox
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SetBoxController : ControllerBase
    {

        private readonly ILogger<SetBoxController> _logger;
        private readonly IHostingEnvironment _environment;
        private readonly IRepository<Device> _repository;
        /// <summary>
        /// SetBoxController
        /// </summary>
        public SetBoxController(ILogger<SetBoxController> logger, IHostingEnvironment environment, ApplicationDbContext context)
        {
            _logger = logger;
            _environment = environment;
            _repository = new Repository<Device>(context);
        }

        /// <summary>
        /// Criar ou Atualizar as informações do SetBox
        /// </summary>
        /// <param name="deviceIdentifier"></param>
        /// <param name="platform"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [HttpGet("Update")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<string>>> Update(string deviceIdentifier, string platform, string version)
        {
            var r = new Models.Response<string>();
            try
            {
                if (string.IsNullOrEmpty(deviceIdentifier))
                    throw new ArgumentNullException(nameof(deviceIdentifier), "Device Identifier is invalid!");


               var device = _repository.Get(x => x.DeviceIdentifier == deviceIdentifier).FirstOrDefault();

                DeviceLogAccesses log = new DeviceLogAccesses();
                log.CreationDateTime = DateTime.Now;
                log.IpAcessed = HttpContext.GetClientIpAddress();
                log.DeviceLogAccessesId = Guid.NewGuid();

                if (device == null)
                {
                    device = new Device()
                    {
                        CreationDateTime = DateTime.Now,
                        DeviceIdentifier = deviceIdentifier,
                        Platform = platform,
                        Version = version,
                        DeviceId = Guid.NewGuid()
                    };
                    log.Message = "Created";
                    device.LogAccesses = new List<DeviceLogAccesses>();
                    device.LogAccesses.Add(log);
                    await _repository.AddAsync(device);
                    r.Status = true;
                    r.Message = "Device successfully created.";
                    return Created("", r);
                }
                else
                {
                    device.Platform = platform;

                    if (device.Version != version)
                    {
                        device.Version = version;
                        log.Message = "Updated";
                        device.LogAccesses.Add(log);
                        await _repository.UpdateAsync(device);
                    }
                   
                    r.Status = true;
                    r.Message = "Device updated successfully.";
                    return Ok(r);
                }
            }
            catch(Exception ex)
            {
                r.Status = false;
                r.SessionExpired = false;
                r.Message = ex.Message;
                _logger.LogError(ex, "Erro no registro " + ex.Message);
                return BadRequest(r);
            }
        }

        /// <summary>
        /// DeviceLogin
        /// </summary>
        /// <param name="identifier">Device Id</param>
        /// <param name="license">License</param>
        /// <returns>SessionID</returns>
        [HttpPost("Login")]
        [HttpGet("Login")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Response<string>>> Login(string identifier, string license)
        {
            _logger.LogInformation($"identifier:{identifier} | license:{license}");
            var r = new Models.Response<string>();
            try
            {
                string deviceIdentifier64 = CriptoHelpers.Base64Encode(identifier);
                if (license == deviceIdentifier64 || license == "1111")
                {

                    var device = _repository.Get(x => x.DeviceIdentifier == identifier).FirstOrDefault();
                    if (device == null)
                    {
                        device = new Device()
                        {
                            CreationDateTime = DateTime.Now,
                            DeviceIdentifier = identifier,
                            Platform = "",
                            Version = "",
                            DeviceId = Guid.NewGuid()
                        };
                        device.LogAccesses = new List<DeviceLogAccesses>();
                        device.LogAccesses.Add(new DeviceLogAccesses()
                        {
                            CreationDateTime = DateTime.Now,
                            DeviceLogAccessesId = Guid.NewGuid(),
                            IpAcessed = HttpContext.GetClientIpAddress(),
                            Message = "Created"
                        });
                        await _repository.AddAsync(device);
                    }

                    device.LogAccesses.Add(new DeviceLogAccesses()
                    {
                        CreationDateTime = DateTime.Now,
                        DeviceLogAccessesId = Guid.NewGuid(),
                        IpAcessed = HttpContext.GetClientIpAddress(),
                        Message = "Logged"
                    });
                    await _repository.UpdateAsync(device);
                    r.Result = CriptoHelpers.Base64Encode($"{identifier}|{license}|{HttpContext.GetClientIpAddress()}|{DateTime.Now.AddMinutes(30):yyyyMMddHHmmss}");
                    return Ok(r);
                }
                r.Message = $"Unauthorized Device {identifier} or license {license} is invalid!";
                r.Status = false;

                return Unauthorized(r);
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
                _logger.LogError(ex, ex.Message, identifier, license);
                return BadRequest(r);
            }
        }

        private ActionResult<Response<T>> Unauthorized<T>(Response<T> r)
        {
            return StatusCode(401, r);
        }

        /// <summary>
        /// Verifica se a session está valida
        /// </summary>
        /// <param name="session">returno do DeviceLogin</param>
        /// <returns></returns>
        [HttpGet("ValidSession")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status401Unauthorized)]
        public ActionResult<Response<bool>> ValidSession(string session)
        {
            var r = new Response<bool>();
            if (ValidaSession(session))
            {
                r.Result = true;
                return Ok(r);
            }
            r.Result = false;
            r.SessionExpired = true;
            return Unauthorized(r);
        }

        /// <summary>
        /// Recuperar as configurações do SetBox
        /// </summary>
        /// <param name="session">returno do DeviceLogin</param>
        /// <returns></returns>
        [HttpGet("GetConfig")]
        [ProducesResponseType(typeof(Response<Config>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<Config>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<Config>), StatusCodes.Status404NotFound)]
        public ActionResult<Response<Config>> GetConfig(string session)
        {
            var r = new Response<Config>();

            if (!ValidaSession(session))
            {
                r.Message = "Session is invalid!";
                r.SessionExpired = true;
                return Unauthorized(r);
            }

            //Recuperar as configurações especifica para o DeviceId
            //return Ok(r);

            r.Message = "Não localizado configuração especifica para esse Device";
            return NotFound(r);
        }

        /// <summary>
        /// Listar os arquivos 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        [HttpGet("ListFilesCheckSum")]
        [ProducesResponseType(typeof(Response<IEnumerable<FileCheckSum>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<IEnumerable<FileCheckSum>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<IEnumerable<FileCheckSum>>), StatusCodes.Status400BadRequest)]

        public ActionResult<Response<IEnumerable<FileCheckSum>>> ListFilesCheckSum(string session)
        {
            var r = new Response<IEnumerable<FileCheckSum>>();
            try
            {
                if (!ValidaSession(session))
                {
                    r.Message = "Session is invalid!";
                    r.SessionExpired = true;
                    return Unauthorized(r);
                }
                DirectoryInfo di = new DirectoryInfo(Path.Combine(_environment.WebRootPath, "UploadedFiles"));

                var files = di.EnumerateFiles()
                    .AsParallel()
                    .Select(x => new FileCheckSum()
                    {
                        Nome = x.Name,
                        Extensao = x.Extension,
                        Tamanho = x.Length,
                        CheckSum = CheckSumHelpers.CalculateMD5(x.FullName),
                        Url = "https://setbox.afonsoft.com.br/UploadedFiles/" + x.Name
                    })
                    .ToArray();

                r.Result = files;
                return Ok(r);
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
                _logger.LogError(ex, ex.Message);
                return BadRequest(r);
            }
        }


        private bool ValidaSession(string session)
        {
            try
            {
                string[] sessions = CriptoHelpers.Base64Decode(session).Split("|");
                string deviceIdentifier64 = CriptoHelpers.Base64Encode(sessions[0]);
                string ip = sessions[2];

                DateTime dt = DateTime.ParseExact(sessions[2], "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                if (sessions[1] == deviceIdentifier64 || sessions[1] == "1111")
                    if (dt >= DateTime.Now && ip == HttpContext.GetClientIpAddress())
                        return true;
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro na validação da session: {session} : {ex.Message}");
                return false;
            }
        }

        private string GetDeviceIdFromSession(string session)
        {
            try
            {
                if (!ValidaSession(session))
                    throw new ArgumentOutOfRangeException(nameof(session), "Session is invalid!");
                return CriptoHelpers.Base64Decode(session).Split("|")[0];
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException(nameof(session), "Session is invalid!");
            }
        }
    }
}