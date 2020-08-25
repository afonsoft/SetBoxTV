using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Afonsoft.SetBox.MultiTenancy.Accounting;
using Afonsoft.SetBox.Web.Areas.App.Models.Accounting;
using Afonsoft.SetBox.Web.Controllers;

namespace Afonsoft.SetBox.Web.Areas.App.Controllers
{
    [Area("App")]
    public class InvoiceController : SetBoxControllerBase
    {
        private readonly IInvoiceAppService _invoiceAppService;

        public InvoiceController(IInvoiceAppService invoiceAppService)
        {
            _invoiceAppService = invoiceAppService;
        }


        [HttpGet]
        public async Task<ActionResult> Index(long paymentId)
        {
            var invoice = await _invoiceAppService.GetInvoiceInfo(new EntityDto<long>(paymentId));
            var model = new InvoiceViewModel
            {
                Invoice = invoice
            };

            return View(model);
        }
    }
}