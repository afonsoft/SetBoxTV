using System;
using System.Collections.Generic;
using System.Text;

namespace VideoPlayerProima.Model
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class SupportModel
    {
        /*
            supportId	string($uuid)
            company	string
            telephone	string
            email	string
            name	string
            creationDateTime	string($date-time)
         */
        [Newtonsoft.Json.JsonConstructor]
        public SupportModel()
        {
        }
        public Guid supportId { get; set; }
        public string company { get; set; }
        public string telephone { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string creationDateTime { get; set; }
    }
}
