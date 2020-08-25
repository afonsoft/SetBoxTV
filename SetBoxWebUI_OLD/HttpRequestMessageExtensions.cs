using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace SetBoxWebUI
{
    /// <summary>
    /// HttpRequestMessageExtensions
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

        /// <summary>
        /// GetClientIpAddress
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            return "127.0.0.1"; 
        }

        /// <summary>
        /// GetClientIpAddress
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientIpAddress(this HttpContext request)
        {
            if (request.Request.Headers.ContainsKey(HttpContext))
            {
                var ctx = request.Request.Headers[HttpContext];
                if (ctx.Count > 0)
                {
                    return ctx[0].ToString();
                }
            }

            if (request.Request.Headers.ContainsKey(RemoteEndpointMessage))
            {
                var remoteEndpoint = request.Request.Headers[RemoteEndpointMessage];
                if (remoteEndpoint.Count > 0)
                {
                    return remoteEndpoint[0].ToString();
                }
            }

            if (request.Connection != null && request.Connection.RemoteIpAddress != null)
            {
                return request.Connection.RemoteIpAddress.ToString();
            }

            return "127.0.0.1";
        }
    }
}
