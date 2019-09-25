using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SetBoxTV.VideoPlayer.Model;

namespace SetBoxTV.VideoPlayer.API
{
    internal static class WithExtensions
    {
        internal static T With<T>(this T self, Action<T> @do)
        {
            @do(self);
            return self;
        }
    }

    /// <summary>
    /// API
    /// </summary>
    public class SetBoxApi
    {
        private readonly string deviceIdentifier;
        private readonly string license;
        private readonly Afonsoft.Http.Rest rest;

        private string session;

        /// <summary>
        /// Contrutor
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="license"></param>
        /// <param name="endPoint"></param>
        public SetBoxApi(string identifier, string license, string endPoint)
        {
            deviceIdentifier = identifier;
            this.license = license;
            rest = new Afonsoft.Http.Rest(endPoint);
            GetSessionLogin();
        }

        private void GetSessionLogin()
        {
            if (string.IsNullOrEmpty(license))
                return;

            try
            {
                var resp = rest.HttpGet<Response<string>>("/Login", Afonsoft.Http.Parameters.With("identifier", deviceIdentifier).And("license", license));

                if (!resp.sessionExpired && resp.status)
                    session = resp.result;
                else
                    throw new ApiException(resp.message);
            }
            catch (Exception e)
            {
                throw new ApiException(e.Message, e);
            }
        }

        private Task<Response<T>> GetResponse<T>(string endpont)
        {
            return rest.HttpGetAsync<Response<T>>(endpont, Afonsoft.Http.Parameters.With("session", session));
        }

        /// <summary>
        /// GetFilesCheckSums
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<FileCheckSum>> GetFilesCheckSums()
        {
            if (string.IsNullOrEmpty(session))
                return null;

            try
            {
                var resp = await GetResponse<IEnumerable<FileCheckSum>>("/ListFilesCheckSum");

                if (!resp.sessionExpired && resp.status)
                    return resp.result;

                throw new ApiException(resp.message);
            }
            catch (Exception e)
            {
                throw new ApiException(e.Message, e);
            }
        }

        /// <summary>
        /// Get Config from Server
        /// </summary>
        /// <returns></returns>
        public async Task<ConfigModel> GetConfig()
        {
            if (string.IsNullOrEmpty(session))
                return null;

            try
            {
                var resp = await GetResponse<ConfigModel>("/GetConfig");

                if (!resp.sessionExpired && resp.status)
                    return resp.result;

                throw new ApiException(resp.message);
            }
            catch (Exception e)
            {
                throw new ApiException(e.Message, e);
            }
        }

        /// <summary>
        /// Get Config from Server
        /// </summary>
        /// <returns></returns>
        public async Task<ConfigModel> SetConfig(ConfigModel config)
        {
            if (string.IsNullOrEmpty(session))
                return null;

            try
            {
                var resp = await rest.HttpPostAsync<Response<ConfigModel>>("/SetConfig",
                    Afonsoft.Http.Parameters.With("session", session)
                                            .And("enableVideo", config.enableVideo.ToString())
                                            .And("enablePhoto", config.enablePhoto.ToString())
                                            .And("enableWebVideo", config.enableWebVideo.ToString())
                                            .And("enableWebImage", config.enableWebImage.ToString())
                                            .And("enableTransaction", config.enableTransaction.ToString())
                                            .And("transactionTime", config.transactionTime.ToString()));

                if (!resp.sessionExpired && resp.status)
                    return resp.result;

                throw new ApiException(resp.message);
            }
            catch (Exception e)
            {
                throw new ApiException(e.Message, e);
            }
        }

        /// <summary>
        /// Update information from SetBox
        /// </summary>
        /// <returns></returns>
        public async Task<string> Update(string platform, string version, string apkVersion, string model, string manufacturer, string deviceName)
        {
            if (string.IsNullOrEmpty(session))
                return null;

            try
            {
                var resp = await rest.HttpPostAsync<Response<string>>("/Update",
                    Afonsoft.Http.Parameters.With("session", session)
                                            .And("platform", platform)
                                            .And("version", version)
                                            .And("apkVersion", apkVersion)
                                            .And("model", model)
                                            .And("manufacturer", manufacturer)
                                            .And("deviceName", deviceName));

                if (!resp.sessionExpired && resp.status)
                    return resp.result;

                throw new ApiException(resp.message);
            }
            catch (Exception e)
            {
                throw new ApiException(e.Message, e);
            }
        }


        /// <summary>
        /// Get Support Info for display
        /// </summary>
        /// <returns></returns>
        public async Task<SupportModel> GetSupport()
        {
            if (string.IsNullOrEmpty(session))
                return null;

            try
            {
                var resp = await GetResponse<SupportModel>("/GetSupport");

                if (!resp.sessionExpired && resp.status)
                    return resp.result;

                throw new ApiException(resp.message);
            }
            catch (Exception e)
            {
                throw new ApiException(e.Message, e);
            }
        }
    }

    /// <summary>
    /// Response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class Response<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        [Newtonsoft.Json.JsonConstructor]
        public Response()
        {
        }
        /// <summary>
        /// objeto de retorno
        /// </summary>
        public T result { get; set; }
        /// <summary>
        /// Mensagem de erro
        /// </summary>
        public string message { get; set; } = "";
        /// <summary>
        /// Sessão invalida ou expirada
        /// </summary>
        public bool sessionExpired { get; set; } = false;
        /// <summary>
        /// Teve erro?
        /// </summary>
        public bool status { get; set; } = true;
    }

    /// <summary>
    /// SessionException
    /// </summary>
    public class ApiException : ArgumentException
    {

        /// <summary>
        /// SessionException
        /// </summary>
        /// <param name="message"></param>
        public ApiException(string message) : base(message)
        {

        }

        /// <summary>
        /// SessionException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ApiException(string message, Exception innerException) : base(message, innerException)
        {

        }

        /// <summary>
        /// SessionException
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}