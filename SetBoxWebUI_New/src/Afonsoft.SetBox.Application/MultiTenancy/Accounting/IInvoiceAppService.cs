using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Afonsoft.SetBox.MultiTenancy.Accounting.Dto;

namespace Afonsoft.SetBox.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
