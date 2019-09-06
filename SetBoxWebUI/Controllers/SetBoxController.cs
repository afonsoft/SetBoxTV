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
        private const string DefaultLicense = "1111";
        private readonly ILogger<SetBoxController> _logger;
        private readonly IRepository<Device> _devices;
        private readonly IRepository<DeviceFiles> _deviceFiles;

        /// <summary>
        /// SetBoxController
        /// </summary>
        public SetBoxController(ILogger<SetBoxController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _devices = new Repository<Device>(context);
            _deviceFiles = new Repository<DeviceFiles>(context);
        }


        /// <summary>
        /// Criar ou Atualizar as informações do SetBox
        /// </summary>
        /// <param name="session"></param>
        /// <param name="platform"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [HttpGet("Update")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<string>>> Update(string session, string platform, string version)
        {
            var r = new Models.Response<string>();
            try
            {
                if (!ValidaSession(session))
                {
                    r.Message = "Session is invalid!";
                    r.SessionExpired = true;
                    return Unauthorized(r);
                }

                string deviceIdentifier = GetDeviceIdFromSession(session);
                string deviceLicense = GetLocenseFromSession(session);

                var device = _devices.Get(x => x.DeviceIdentifier == deviceIdentifier).FirstOrDefault();

                if (device == null)
                {
                    r.Status = false;
                    r.SessionExpired = false;
                    r.Message = $"Device '{deviceIdentifier}' not found";
                    _logger.LogError($"Device '{deviceIdentifier}' not found");
                    return NotFound(r);
                }

                if (device.Version != version || device.License != deviceLicense || device.Platform != platform)
                {
                    DeviceLogAccesses log = new DeviceLogAccesses();
                    log.CreationDateTime = DateTime.Now;
                    log.IpAcessed = HttpContext.GetClientIpAddress();
                    log.DeviceLogAccessesId = Guid.NewGuid();

                    device.Platform = platform;
                    device.Version = version;
                    device.License = deviceLicense;
                    log.Message = "Updated";
                    device.LogAccesses.Add(log);
                    await _devices.UpdateAsync(device);
                    r.Status = true;
                    r.Message = "Device updated successfully.";
                    return Ok(r);
                }

                r.Status = true;
                r.Message = "Device not need update.";
                return Ok(r);

            }
            catch (Exception ex)
            {
                r.Status = false;
                r.SessionExpired = false;
                r.Message = ex.Message;
                _logger.LogError(ex, ex.Message);
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
                if (license == deviceIdentifier64 || license == DefaultLicense)
                {

                    var device = _devices.Get(x => x.DeviceIdentifier == identifier).FirstOrDefault();
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
                        await _devices.AddAsync(device);
                    }

                    device.LogAccesses.Add(new DeviceLogAccesses()
                    {
                        CreationDateTime = DateTime.Now,
                        DeviceLogAccessesId = Guid.NewGuid(),
                        IpAcessed = HttpContext.GetClientIpAddress(),
                        Message = "Logged"
                    });
                    await _devices.UpdateAsync(device);
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
        /// Salvar as configurações no Banco de Dados
        /// </summary>
        /// <param name="session"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        [HttpGet("SetConfig")]
        [ProducesResponseType(typeof(Response<Config>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<Config>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<Config>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<Config>>> SetConfig(string session, Config config)
        {
            var r = new Response<Config>();
            try
            {
                if (!ValidaSession(session))
                {
                    r.Message = "Session is invalid!";
                    r.SessionExpired = true;
                    return Unauthorized(r);
                }
                string identifier = GetDeviceIdFromSession(session);
                //Recuperar as configurações especifica para o DeviceId
                var device = _devices.Get(x => x.DeviceIdentifier == identifier).FirstOrDefault();

                if (device == null)
                {
                    r.Status = false;
                    r.Message = "Not Found Configuration Specifies for this Device.";
                    return NotFound(r);
                }

                device.Configuration = config;
                device.LogAccesses.Add(new DeviceLogAccesses()
                {
                    CreationDateTime = DateTime.Now,
                    DeviceLogAccessesId = Guid.NewGuid(),
                    IpAcessed = HttpContext.GetClientIpAddress(),
                    Message = "Update Config"
                });

                await _devices.UpdateAsync(device);

                r.Result = device.Configuration;
                r.Status = true;
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
            try
            {
                if (!ValidaSession(session))
                {
                    r.Message = "Session is invalid!";
                    r.SessionExpired = true;
                    return Unauthorized(r);
                }
                string identifier = GetDeviceIdFromSession(session);
                //Recuperar as configurações especifica para o DeviceId
                var device = _devices.Get(x => x.DeviceIdentifier == identifier).FirstOrDefault();

                if (device == null || device.Configuration == null)
                {
                    r.Status = false;
                    r.Message = "Not Found Configuration Specifies for this Device.";
                    return NotFound(r);
                }

                r.Result = device.Configuration;
                r.Status = true;
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

        /// <summary>
        /// Listar os arquivos 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        [HttpGet("ListFilesCheckSum")]
        [ProducesResponseType(typeof(Response<IEnumerable<FileCheckSum>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<IEnumerable<FileCheckSum>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<IEnumerable<FileCheckSum>>), StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<Response<IEnumerable<FileCheckSum>>>> ListFilesCheckSum(string session)
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

                string identifier = GetDeviceIdFromSession(session);

               var files = await _deviceFiles.GetAsync(x => x.Device.DeviceIdentifier == identifier);
                
                List<FileCheckSum> itens = new List<FileCheckSum>();
                foreach (var item in files)
                {
                    foreach (var file in item.Files)
                    {
                        itens.Add(new FileCheckSum()
                        {
                            Name = file.Name,
                            Extension = file.Extension,
                            Size = file.Size,
                            CheckSum = file.CheckSum,
                            Url = "https://setbox.afonsoft.com.br/UploadedFiles/" + file.Name
                        });
                    }
                }

                r.Result = itens;
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


        private ActionResult<Response<T>> Unauthorized<T>(Response<T> r)
        {
            return StatusCode(401, r);
        }
        private bool ValidaSession(string session)
        {
            try
            {
                string[] sessions = CriptoHelpers.Base64Decode(session).Split("|");
                string deviceIdentifier64 = CriptoHelpers.Base64Encode(sessions[0]);
                string ip = sessions[2];

                DateTime dt = DateTime.ParseExact(sessions[2], "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                if (sessions[1] == deviceIdentifier64 || sessions[1] == DefaultLicense)
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
        private string GetLocenseFromSession(string session)
        {
            try
            {
                if (!ValidaSession(session))
                    throw new ArgumentOutOfRangeException(nameof(session), "Session is invalid!");
                return CriptoHelpers.Base64Decode(session).Split("|")[1];
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException(nameof(session), "Session is invalid!");
            }
        }
    }
}