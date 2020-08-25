using Abp.Application.Services.Dto;

namespace Afonsoft.SetBox.SetBox.Dto
{
    public class LogErrorDto : EntityDto<long>
    {
        public string IpAcessed { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Level { get; set; }
    }
}
