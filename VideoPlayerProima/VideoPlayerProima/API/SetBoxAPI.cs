using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VideoPlayerProima.Model;

namespace VideoPlayerProima.API
{
    public class SetBoxAPI
    {
        private readonly string deviceIdentifier;
        private readonly string license;
        private readonly string endPoint;
        private readonly Afonsoft.Http.Rest rest;

        private string session;

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
            rest.AddParameter("identifier", deviceIdentifier);
            rest.AddParameter("license", license);
            session = rest.HttpGet("/Login");
        }

        public Task<IEnumerable<FileCheckSum>> GetFilesCheckSums()
        {
            rest.AddParameter("session", session);
            return rest.HttpGetAsync<IEnumerable<FileCheckSum>>("/ListFilesCheckSum");
        }
    }
}
