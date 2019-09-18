using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SetBoxWebUI.Interfaces;
using SetBoxWebUI.Models;
using SetBoxWebUI.Models.Views;
using SetBoxWebUI.Repository;

namespace SetBoxWebUI.Controllers
{
    [Authorize]
    public class DevicesController : BaseController
    {
        private readonly ILogger<DevicesController> _logger;
        private readonly IRepository<Device, Guid> _devices;

        /// <summary>
        /// SetBoxController
        /// </summary>
        public DevicesController(ILogger<DevicesController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _devices = new Repository<Device, Guid>(context);
        }
       
        public IActionResult Index(DeviceViewModel model)
        {
            ViewData["Edit"] = false;
            if (model == null) model = new DeviceViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string command)
        {
            try
            {
                ViewData["Edit"] = true;
                ViewData["Command"] = command;
                var item = await _devices.FirstOrDefaultAsync(x => x.DeviceId.ToString() == id);

                if (item == null)
                    throw new KeyNotFoundException($"DeviceId: {id} not found.");

                DeviceViewModel model = new DeviceViewModel
                {
                    IsEdited = command == "Edit",
                    DeviceId = item.DeviceId,
                    DeviceIdentifier = item.DeviceIdentifier,
                    CreationDateTime = item.CreationDateTime,
                    License = item.License,
                    Platform = item.Platform,
                    Version = item.Version,
                    Name = item.Name
                };
                if (item.Configuration != null)
                {
                    model.TransactionTime = item.Configuration.TransactionTime;
                    model.EnablePhoto = item.Configuration.EnablePhoto;
                    model.EnableTransaction = item.Configuration.EnableTransaction;
                    model.EnableVideo = item.Configuration.EnableVideo;
                    model.EnableWebImage = item.Configuration.EnableWebImage;
                    model.EnableWebVideo = item.Configuration.EnableWebVideo;
                    model.ConfigId = item.Configuration.ConfigId;
                }
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                ViewData["Edit"] = false;
                return View("Index", new DeviceViewModel(ex));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(DeviceViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var updItem = await _devices.FirstOrDefaultAsync(x => x.DeviceId == model.DeviceId);

                    if (updItem != null)
                    {
                        updItem.Name = model.Name;

                        if (updItem.Configuration == null)
                        {
                            updItem.Configuration = new Config
                            {
                                CreationDateTime = DateTime.Now
                            };
                        }
                        updItem.Configuration.EnablePhoto = model.EnablePhoto;
                        updItem.Configuration.EnableTransaction = model.EnableTransaction;
                        updItem.Configuration.EnableVideo = model.EnableVideo;
                        updItem.Configuration.EnableWebImage = model.EnableWebImage;
                        updItem.Configuration.EnableWebVideo = model.EnableWebVideo;
                        updItem.Configuration.TransactionTime = model.TransactionTime;

                        await _devices.UpdateAsync(updItem);

                        ViewData["Edit"] = false;
                        return RedirectToAction("Index", new DeviceViewModel("Device update was successful."));
                    }
                    else
                    {
                        ViewData["Edit"] = false;
                        throw new KeyNotFoundException($"DeviceId: {model.DeviceId} not found.");
                    }
                }
                ViewData["Edit"] = false;
                return View("Index", new DeviceViewModel());

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                ViewData["Edit"] = false;
                return View("Index", new DeviceViewModel(ex));
            }
        }
        //Device

        public async Task<GridPagedOutput<FileCheckSum>> ListFiles(GridPagedInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.SearchPhrase))
                    input.SearchPhrase = "";

                if (string.IsNullOrEmpty(input.Id))
                    throw new KeyNotFoundException($"DeviceId: {input.Id} not found.");

                var devices = await _devices.GetAsync(f => f.DeviceId.ToString() == input.Id);

                if (devices.Count <= 0)
                    throw new KeyNotFoundException($"DeviceId: {input.Id} not found.");

                var files = devices[0].Files.Where(l => (l.FileId.ToString() == input.Id
                                                        || l.DeviceId.ToString() == input.Id)
                                                    && (l.CreationDateTime.ToString("dd/MM/yyyy").Contains(input.SearchPhrase)
                                                        || l.File.Name.Contains(input.SearchPhrase)
                                                        || l.File.Path.Contains(input.SearchPhrase)
                                                        || l.File.Extension.Contains(input.SearchPhrase)))
                                            .Select(x => x.File)
                                            .Skip((input.Current - 1) * input.RowCount)
                                            .Take(input.RowCount)
                                            .ToList();

                var item = new GridPagedOutput<FileCheckSum>(files) { Current = input.Current, RowCount = input.RowCount, Total = devices[0].Files.Count };
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        public async Task<GridPagedOutput<DeviceLogAccesses>> ListLog(GridPagedInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.SearchPhrase))
                    input.SearchPhrase = "";

                if (string.IsNullOrEmpty(input.Id))
                    throw new KeyNotFoundException($"DeviceId: {input.Id} not found.");

                var devices = await _devices.GetAsync(f => f.DeviceId.ToString() == input.Id);

                if (devices.Count <= 0)
                    throw new KeyNotFoundException($"DeviceId: {input.Id} not found.");

                var logs = devices[0].LogAccesses.Where(l => l.DeviceLogAccessesId.ToString() == input.Id
                                                        || l.CreationDateTime.ToString("dd/MM/yyyy").Contains(input.SearchPhrase)
                                                        || l.IpAcessed.Contains(input.SearchPhrase)
                                                        || l.Message.Contains(input.SearchPhrase))
                                                 .Skip((input.Current - 1) * input.RowCount)
                                                 .Take(input.RowCount)
                                                 .ToList();

                var item = new GridPagedOutput<DeviceLogAccesses>(logs) { Current = input.Current, RowCount = input.RowCount, Total = devices[0].LogAccesses.Count };
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<GridPagedOutput<Device>> List(GridPagedInput input)
        {

            try
            {
                if (string.IsNullOrEmpty(input.SearchPhrase))
                    input.SearchPhrase = "";

                Expression<Func<Device, object>> orderby = o => o.DeviceIdentifier;
                var keys = input.Sort.OrderBy(kvp => kvp.Key).First();

                switch (keys.Key)
                {
                    case "deviceIdentifier":
                        orderby = o => o.DeviceIdentifier;
                        break;
                    case "license":
                        orderby = o => o.License;
                        break;
                    case "platform":
                        orderby = o => o.Platform;
                        break;
                    case "version":
                        orderby = o => o.Version;
                        break;
                    case "name":
                        orderby = o => o.Name;
                        break;
                }

                var itens = new List<Device>();
                var devices = await _devices.GetPagination(f => f.DeviceId.ToString() == input.Id
                                           || f.DeviceIdentifier.Contains(input.SearchPhrase)
                                           || f.License.Contains(input.SearchPhrase)
                                           || f.Platform.Contains(input.SearchPhrase)
                                           || f.Version.Contains(input.SearchPhrase)
                                           || f.Name.Contains(input.SearchPhrase)
                                           || f.CreationDateTime.ToString("dd/MM/yyyy").Contains(input.SearchPhrase),
                                            keys.Value == "asc" ? orderby : null,
                                            keys.Value == "desc" ? orderby : null,
                                         input.Current,
                                         input.RowCount);

                itens.AddRange(devices.Value);
                var item = new GridPagedOutput<Device>(itens) { Current = input.Current, RowCount = input.RowCount, Total = devices.Key };
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<string> Delete(string id)
        {
            try
            {
                var del = await _devices.FirstOrDefaultAsync(x => x.DeviceId.ToString() == id);
                if (del != null)
                {
                   await _devices.DeleteAsync(del);
                    return "Device Deleted!";
                }
                return "Device Not Found!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ex.Message;
            }
        }
    }
}