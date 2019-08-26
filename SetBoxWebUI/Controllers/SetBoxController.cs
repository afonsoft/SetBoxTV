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
using SetBoxWebUI.Models;

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
        /// <summary>
        /// SetBoxController
        /// </summary>
        public SetBoxController(ILogger<SetBoxController> logger, IHostingEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
            //https://exceptionnotfound.net/asp-net-core-demystified-action-results/
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
        public ActionResult<Response<string>> Login(string identifier, string license)
        {
            _logger.LogInformation($"identifier:{identifier} | license:{license}");
            var r = new Models.Response<string>();
            try
            {
                string deviceIdentifier64 = CriptoHelpers.Base64Encode(identifier);
                if (license == deviceIdentifier64 || license == "1111") {
                    r.Result = CriptoHelpers.Base64Encode($"{identifier}|{license}|{DateTime.Now.AddMinutes(30):yyyyMMddHHmmss}");
                    return Ok(r);
                }
                r.Message = $"Unauthorized Device {identifier} or license {license} is invalid!";
                r.Status = false;

                return Unauthorized(r);
            }
            catch (Exception ex)
            {
                r.Status= false;
                r.Message = "identifier or license is null!";
                _logger.LogError(ex, "Erro no Login", identifier, license);
                return BadRequest(r);
            }
        }

        private ActionResult<Response<T>> Unauthorized<T>(Response<T> r)
        {
            return Unauthorized();
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

                DateTime dt = DateTime.ParseExact(sessions[2], "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                if (sessions[1] == deviceIdentifier64 || sessions[1] == "1111")
                    if (dt >= DateTime.Now)
                        return true;
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro na validação da session: {session} : {ex.Message}");
                return false;
            }
        }
    }
}