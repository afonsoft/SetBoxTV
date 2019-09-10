using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetBoxWebUI.Models;

namespace SetBoxWebUI.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public GridPagedOutput<Device> List(GridPagedInput input)
        {
            var itens = new List<Device>();
            itens.Add(new Device() { CreationDateTime = DateTime.Now, DeviceIdentifier = "ABCD", License = "1111", Platform = "Android", Version = "6", DeviceId= Guid.NewGuid() });
            itens.Add(new Device() { CreationDateTime = DateTime.Now, DeviceIdentifier = "QWEADS", License = "1111", Platform = "Android", Version = "6.1", DeviceId = Guid.NewGuid() });
            itens.Add(new Device() { CreationDateTime = DateTime.Now, DeviceIdentifier = "34234", License = "1111", Platform = "Android", Version = "7.1", DeviceId = Guid.NewGuid() });

            var item = new GridPagedOutput<Device>(itens) {Current = input.Current, RowCount = input.RowCount, Total = 3 };
    
            return item;
        }
    }
}