using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VideoPlayerProima.Model;

namespace VideoPlayerProima.API
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
    public class SetBoxAPI
    {
        private readonly string deviceIdentifier;
        private readonly string license;
        private readonly string endPoint;
        private readonly Afonsoft.Http.Rest rest;

        private string session;

        /// <summary>
        /// Contrutor
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="license"></param>
        /// <param name="endPoint"></param>
        public SetBoxAPI(string identifier, string license, string endPoint)
        {
            deviceIdentifier = identifier;
            this.license = license;
            this.endPoint = endPoint;
            rest = new Afonsoft.Http.Rest(this.endPoint);
            GetSessionLogin();
        }

        private void GetSessionLogin()
        {
            session = rest.HttpGet("/Login", Afonsoft.Http.Parameters.With("identifier", deviceIdentifier).And("license", license));
        }

        /// <summary>
        /// GetFilesCheckSums
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<FileCheckSum>> GetFilesCheckSums()
        {
            return rest.HttpGetAsync<IEnumerable<FileCheckSum>>("/ListFilesCheckSum", Afonsoft.Http.Parameters.With("session", session));
        }
    }
}
