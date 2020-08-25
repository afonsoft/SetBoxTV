using Abp.Application.Services.Dto;

namespace Afonsoft.SetBox.SetBox.Dto
{
    public class LogAccessesDto : EntityDto<long>
    {
        public string IpAcessed { get; set; }
        public string Message { get; set; }
    }
}
