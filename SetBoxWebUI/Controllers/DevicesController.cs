using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SetBoxWebUI.Interfaces;
using SetBoxWebUI.Models;
using SetBoxWebUI.Repository;

namespace SetBoxWebUI.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {
        private readonly ILogger<DevicesController> _logger;
        private readonly IRepository<Device> _devices;

        /// <summary>
        /// SetBoxController
        /// </summary>
        public DevicesController(ILogger<DevicesController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _devices = new Repository<Device>(context);
        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<GridPagedOutput<Device>> List(GridPagedInput input)
        {
            if (string.IsNullOrEmpty(input.SearchPhrase))
                input.SearchPhrase = "";

            var itens = new List<Device>();
            var devices = await _devices.GetPagination(f => f.DeviceId.ToString() == input.Id
                                       || f.DeviceIdentifier.Contains(input.SearchPhrase)
                                       || f.License.Contains(input.SearchPhrase)
                                       || f.Platform.Contains(input.SearchPhrase)
                                       || f.Version.Contains(input.SearchPhrase),
                                     input.Current,
                                     input.RowCount);

            itens.AddRange(devices.Value);
            var item = new GridPagedOutput<Device>(itens) {Current = input.Current, RowCount = input.RowCount, Total = devices.Key };
            return item;
        }

        public Device Edit(string id)
        {
            return _devices.Get(x => x.DeviceId.ToString() == id).FirstOrDefault();
        }

        public Config Config(string id)
        {
            return _devices.Get(x => x.DeviceId.ToString() == id).FirstOrDefault()?.Configuration;
        }

        public bool Delete(string id)
        {
            var del = _devices.Get(x => x.DeviceId.ToString() == id).FirstOrDefault();

            if (del != null)
            {
                _devices.Delete(del);
                return true;
            }
            return false;
        }
    }
}