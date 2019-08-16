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
        }
        /// <summary>
        /// DeviceLogin
        /// </summary>
        /// <param name="identifier">Device Id</param>
        /// <param name="license">License</param>
        /// <returns>SessionID</returns>
        [HttpPost("Login")]
        [HttpGet("Login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult<string> Login(string identifier, string license)
        {
            _logger.LogInformation($"identifier:{identifier} | license:{license}");
            string deviceIdentifier64 = CriptoHelpers.Base64Encode(identifier);
            if (license == deviceIdentifier64 || license == "1111")
                return Ok(CriptoHelpers.Base64Encode($"{identifier}|{license}|{DateTime.Now.AddMinutes(30):yyyyMMddHHmmss}"));
            return BadRequest("Unauthorized");
        }

        /// <summary>
        /// Verifica se a session está valida
        /// </summary>
        /// <param name="session">returno do DeviceLogin</param>
        /// <returns></returns>
        [HttpGet("ValidSession")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
        public ActionResult<bool> ValidSession(string session)
        {
            if (ValidaSession(session))
                return Ok(true);
            return BadRequest(false);
        }

        /// <summary>
        /// Listar os arquivos 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        [HttpGet("ListFilesCheckSum")]
        [ProducesResponseType(typeof(IEnumerable<FileCheckSum>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]

        public ActionResult<IEnumerable<FileCheckSum>> ListFilesCheckSum(string session)
        {
            try
            {
                if (!ValidaSession(session))
                    return BadRequest("Session is invalid!");

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

                return Ok(files);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                _logger.LogInformation($"Erro na validação da session: {session} : {ex.Message}");
                return false;
            }
        }
    }
}