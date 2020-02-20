using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SetBoxTV.VideoPlayer.Helpers;
using SetBoxTV.VideoPlayer.Model;
using Xamarin.Forms.Internals;

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


    public enum LogLevel
    {
        INFO,
        ERROR,
        DEBUG
    }
    /// <summary>
    /// API
    /// </summary>
    public class SetBoxApi
    {
        private readonly string deviceIdentifier;
        private string _license;
        private readonly Afonsoft.Http.Rest rest;
        private string _session;

        public string License
        {
            get => _license;
            private set { _license = value; }
        }

        public string Session
        {
            get => _session;
            private set { _session = value; }
        }

        public static bool CheckConnectionPing(string url)
        {
            string CheckUrl = url;

            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);
                iNetRequest.Timeout = 5000;
                WebResponse iNetResponse = iNetRequest.GetResponse();
                iNetResponse.Close();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Contrutor
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="license"></param>
        /// <param name="endPoint"></param>
        public SetBoxApi(string identifier, string license, string endPoint)
        {
            deviceIdentifier = identifier;
            License = license;

            if (string.IsNullOrEmpty(License))
                License = "1111";

            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentNullException(nameof(identifier), $"identifier {identifier} is null or invalid!");

            rest = new Afonsoft.Http.Rest(endPoint);
            GetSessionLogin();
        }

        private void GetSessionLogin()
        {
            try
            {
                if (string.IsNullOrEmpty(deviceIdentifier))
                {
                    Session = "";
                    License = "";
                    return;
                }
                Response<string> resp;
                if (string.IsNullOrEmpty(License))
                    resp = rest.HttpGet<Response<string>>("/Login", Afonsoft.Http.Parameters.With("identifier", deviceIdentifier));
                else
                    resp = rest.HttpGet<Response<string>>("/Login", Afonsoft.Http.Parameters.With("identifier", deviceIdentifier).And("license", License));

                if (!resp.sessionExpired && resp.status)
                {
                    Session = resp.result;
                    try
                    {
                        License = CriptoHelpers.Base64Decode(CriptoHelpers.Base64Decode(Session).Split('|')[1]);
                    }
                    catch(Exception ex)
                    {
                        License = "";
                        throw new ApiException(ex.Message, ex);
                    }
                }
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
            return rest.HttpGetAsync<Response<T>>(endpont, Afonsoft.Http.Parameters.With("session", Session));
        }

        /// <summary>
        /// GetFilesCheckSums
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<FileCheckSum>> GetFilesCheckSums()
        {
            if (string.IsNullOrEmpty(Session))
                return null;

            try
            {
                var resp = await GetResponse<IEnumerable<FileCheckSum>>("/ListFilesCheckSum").ConfigureAwait(true);

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
            if (string.IsNullOrEmpty(Session))
                return null;

            try
            {
                var resp = await GetResponse<ConfigModel>("/GetConfig").ConfigureAwait(true);

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
            if (string.IsNullOrEmpty(Session))
                return null;

            if (config == null)
                return null;

            try
            {
                var resp = await rest.HttpPostAsync<Response<ConfigModel>>("/SetConfig",
                    Afonsoft.Http.Parameters.With("session", Session)
                                            .And("enableVideo", config.enableVideo.ToString(CultureInfo.InvariantCulture))
                                            .And("enablePhoto", config.enablePhoto.ToString(CultureInfo.InvariantCulture))
                                            .And("enableWebVideo", config.enableWebVideo.ToString(CultureInfo.InvariantCulture))
                                            .And("enableWebImage", config.enableWebImage.ToString(CultureInfo.InvariantCulture))
                                            .And("enableTransaction", config.enableTransaction.ToString(CultureInfo.InvariantCulture))
                                            .And("transactionTime", config.transactionTime.ToString(CultureInfo.InvariantCulture))).ConfigureAwait(true);

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
        /// Log de Erros
        /// </summary>
        /// <param name="mensage"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public async Task<string> Log(string mensage, LogLevel level = LogLevel.ERROR)
        {
            if (string.IsNullOrEmpty(Session))
                return null;

            try
            {
                var resp = await rest.HttpPostAsync<Response<string>>("/Log",
                    Afonsoft.Http.Parameters.With("session", Session)
                                            .And("mensage", mensage)
                                            .And("level", level.ToString().ToUpper(CultureInfo.InvariantCulture))).ConfigureAwait(true);

                if (!resp.sessionExpired && resp.status)
                    return resp.result;

                return resp.message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Update information from SetBox
        /// </summary>
        /// <returns></returns>
        public async Task<string> UpdateInfo(string platform, string version, string apkVersion, string model, string manufacturer, string deviceName, string setboxName)
        {
            if (string.IsNullOrEmpty(Session))
                return null;

            try
            {
                var resp = await rest.HttpPostAsync<Response<string>>("/UpdateInfo",
                    Afonsoft.Http.Parameters.With("session", Session)
                                            .And("platform", platform)
                                            .And("version", version)
                                            .And("apkVersion", apkVersion)
                                            .And("model", model)
                                            .And("manufacturer", manufacturer)
                                            .And("deviceName", deviceName)
                                            .And("setboxName", setboxName)).ConfigureAwait(true);

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
            if (string.IsNullOrEmpty(Session))
                return null;

            try
            {
                var resp = await GetResponse<SupportModel>("/GetSupport").ConfigureAwait(true);

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
    [Preserve(AllMembers = true)]
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

        public ApiException()
        {
        }
    }
}