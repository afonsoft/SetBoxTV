using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Afonsoft.SetBox.Authorization;
using Afonsoft.SetBox.SetBox;
using Afonsoft.SetBox.SetBox.Dto;
using Afonsoft.SetBox.SetBox.Input;
using Afonsoft.SetBox.Web.Areas.App.Models.SetBox;
using Afonsoft.SetBox.Web.Controllers;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Afonsoft.SetBox.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_SetBox)]
    public class SetBoxController : SetBoxControllerBase
    {
        private readonly ISetBoxApiAppService _setBoxApiAppService;
        private readonly ILogger _logger;

        public SetBoxController(ISetBoxApiAppService setBoxApiAppService, ILogger logger)
        {
            _setBoxApiAppService = setBoxApiAppService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<bool> UpdateOrderFiles(string id, string order)
        {
            try
            {
                if (!string.IsNullOrEmpty(order) && !string.IsNullOrEmpty(id))
                {
                    string[] orders = order.Replace("[]=", "-").Split("&");
                    if (orders.Length > 0)
                    {
                        var device = await _setBoxApiAppService.GetDevices(new SetBox.Input.DeviceInput() { Id = long.Parse(id) });
                        if (device != null)
                        {
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

            return true;
        }

        public async Task<PagedResultDto<LogErrorDto>> ListLogError(LogInput input)
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

                var logs = devices[0].Logs.Where(l => l.DeviceLogId.ToString() == input.Id
                                                        || l.CreationDateTime.ToString("dd/MM/yyyy").Contains(input.SearchPhrase)
                                                        || l.IpAcessed.Contains(input.SearchPhrase)
                                                        || l.Message.Contains(input.SearchPhrase))
                                                 .Skip((input.Current - 1) * input.RowCount)
                                                 .Take(input.RowCount)
                                                 .ToList();

                var item = new GridPagedOutput<DeviceLogError>(logs) { Current = input.Current, RowCount = input.RowCount, Total = devices[0].Logs.Count };
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<PagedResultDto<LogAccessesDto>> ListLog(LogInput input)
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


        public ActionResult Index()
        {
            return View(new DeviceViewModel());
        }

        #region Company
        public ActionResult Company()
        {
            return View(new CompanyViewModel());
        }

        #endregion

        #region Devices
        public ActionResult Devices()
        {
            return View(new DeviceViewModel());
        }

        #endregion

        #region Files
        public ActionResult Files()
        {
            return View(new FilesViewModel());
        }

        #endregion

        #region Support
        public ActionResult Support()
        {
            return View(new SupportViewModel());
        }
        #endregion
    }
}
