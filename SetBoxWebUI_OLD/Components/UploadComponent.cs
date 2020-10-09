using Microsoft.AspNetCore.Mvc;
using SetBoxWebUI.Models.Views;

namespace SetBoxWebUI.Components
{
    public class UploadComponent : ViewComponent
    {
        public UploadComponent()
        {

        }
        public IViewComponentResult Invoke(string Id)
        {
            return View(new FileUploadViewModel { Id = Id,  Name = ""});
        }
    }
}
