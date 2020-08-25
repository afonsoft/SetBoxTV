using System;
using Abp.AutoMapper;
using Afonsoft.SetBox.Sessions.Dto;

namespace Afonsoft.SetBox.Models.Common
{
    [AutoMapFrom(typeof(ApplicationInfoDto)),
     AutoMapTo(typeof(ApplicationInfoDto))]
    public class ApplicationInfoPersistanceModel
    {
        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}