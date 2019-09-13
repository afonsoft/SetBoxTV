using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
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
        private readonly IRepository<Device, Guid> _devices;


        /// <summary>
        /// SetBoxController
        /// </summary>
        public SetBoxController(ILogger<SetBoxController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _devices = new Repository<Device, Guid>(context);

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
                ValidaSession(session);

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
                    log.Message = "Device Updated";

                    if(device.Platform != platform)
                        log.Message += $"Platform: {platform} ({device.Platform}) ";

                    if (device.License != deviceLicense)
                        log.Message += $"License: {deviceLicense} ({device.License}) ";

                    if (device.Version != version)
                        log.Message += $"Version: {version} ({device.Version}) ";

                    device.Platform = platform;
                    device.Version = version;
                    device.License = deviceLicense;
                    
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
            catch (SessionException e)
            {
                r.Status = false;
                r.SessionExpired = true;
                r.Message = e.Message;
                return Unauthorized(r);
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
                            License = license,
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

                    r.Result = CriptoHelpers.Base64Encode($"{identifier}|{CriptoHelpers.Base64Encode(license)}|{HttpContext.GetClientIpAddress()}|{DateTime.Now.AddMinutes(30):yyyyMMddHHmmss}|{device.DeviceId}");

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
            try
            {
                ValidaSession(session);

                r.Result = true;
                return Ok(r);
            }
            catch (SessionException e)
            {
                r.Status = false;
                r.SessionExpired = true;
                r.Message = e.Message;
                return Unauthorized(r);
            }
        }

        /// <summary>
        /// Salvar as configurações no Banco de Dados
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [HttpPost("SetConfig")]
        [HttpGet("SetConfig")]
        [ProducesResponseType(typeof(Response<Config>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<Config>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<Config>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<Config>>> SetConfig([FromQuery]ConfigApi config)
        {
            var r = new Response<Config>();
            try
            {
                if (config == null)
                {
                    r.Message = "Config is invalid!";
                    r.SessionExpired = false;
                    r.Status = false;
                    return BadRequest(r);
                }

                ValidaSession(config.session);

                string identifier = GetDeviceIdFromSession(config.session);
                //Recuperar as configurações especifica para o DeviceId
                var device = _devices.Get(x => x.DeviceIdentifier == identifier).FirstOrDefault();

                if (device == null)
                {
                    r.Status = false;
                    r.Message = "Not Found Configuration Specifies for this Device.";
                    return NotFound(r);
                }

                if (device.Configuration == null)
                {
                    device.Configuration = new Config()
                    {
                        ConfigId = Guid.NewGuid(),
                        CreationDateTime = DateTime.Now
                    };
                }

                device.Configuration.EnablePhoto = config.EnablePhoto;
                device.Configuration.EnableTransaction = config.EnableTransaction;
                device.Configuration.EnableVideo = config.EnableVideo;
                device.Configuration.EnableWebImage = config.EnableWebImage;
                device.Configuration.EnableWebVideo = config.EnableWebVideo;
                device.Configuration.TransactionTime = config.TransactionTime;

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
            catch (SessionException e)
            {
                r.Status = false;
                r.SessionExpired = true;
                r.Message = e.Message;
                return Unauthorized(r);
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
                ValidaSession(session);

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
            catch (SessionException e)
            {
                r.Status = false;
                r.SessionExpired = true;
                r.Message = e.Message;
                return Unauthorized(r);
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
                ValidaSession(session);

                string identifier = GetDeviceIdFromSession(session);

                var devices = await _devices.GetAsync(x => x.DeviceIdentifier == identifier);
                var device = devices.FirstOrDefault();

                if (device == null)
                {
                    r.Status = false;
                    r.Message = "Not Found Configuration Specifies for this Device.";
                    return NotFound(r);
                }

                var files = device.Files;

                List<FileCheckSum> itens = new List<FileCheckSum>();
                foreach (var item in files)
                {
                    itens.Add(new FileCheckSum()
                    {
                        Name = item.File.Name,
                        Extension = item.File.Extension,
                        Size = item.File.Size,
                        CheckSum = item.File.CheckSum,
                        Url = "https://setbox.afonsoft.com.br/UploadedFiles/" + item.File.Name
                    });
                }

                r.Result = itens;
                return Ok(r);
            }
            catch (SessionException e)
            {
                r.Status = false;
                r.SessionExpired = true;
                r.Message = e.Message;
                return Unauthorized(r);
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

        private void ValidaSession(string session)
        {
            try
            {
                string[] sessions;
                string deviceIdentifier64;
                string ip;
                string license;
                DateTime dt;
                try
                {
                    sessions = CriptoHelpers.Base64Decode(session).Split("|");
                    deviceIdentifier64 = CriptoHelpers.Base64Encode(sessions[0]);
                    license = CriptoHelpers.Base64Decode(sessions[1]);
                    ip = sessions[2];
                    dt = DateTime.ParseExact(sessions[3], "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    throw new SessionException($"Erro para descriptografar a session {session} Erro : {ex.Message}");
                }


                if (license == deviceIdentifier64 || license == DefaultLicense)
                {
                    if (dt >= DateTime.Now)
                    {
                        if (ip == HttpContext.GetClientIpAddress())
                        {
                            return;
                        }
                        else
                        {
                            throw new SessionException($"O Ip da Session {ip} é diferente do Ip da Request {HttpContext.GetClientIpAddress()}!");
                        }
                    }
                    else
                    {
                        throw new SessionException($"A data da session expirou! data: {dt.ToString("yyyyMMddHHmmss")}");
                    }
                }
                else
                {
                    throw new SessionException($"A Session {session[1]} é inválida!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Session: {session} : {ex.Message}");
                throw;
            }
        }

        private string GetDeviceIdFromSession(string session)
        {
            return CriptoHelpers.Base64Decode(session).Split("|")[0];
        }
        private string GetLocenseFromSession(string session)
        {
            return CriptoHelpers.Base64Decode(session).Split("|")[1];
        }
    }

    /// <summary>
    /// SessionException
    /// </summary>
    public class SessionException : ArgumentException, ISerializable
    {

        /// <summary>
        /// SessionException
        /// </summary>
        /// <param name="message"></param>
        public SessionException(string message) : base(message)
        {

        }

        /// <summary>
        /// SessionException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public SessionException(string message, Exception innerException) : base(message, innerException)
        {

        }

        /// <summary>
        /// SessionException
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SessionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}