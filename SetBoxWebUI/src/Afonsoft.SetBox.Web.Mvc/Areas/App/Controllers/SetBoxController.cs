using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Afonsoft.SetBox.Authorization;
using Afonsoft.SetBox.Dto;
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
                            //_setBoxApiAppService.SetOrderDeviceFile(new OrderDto() { DeviceId = long.Parse(id), fileOrders = new List<FileOrder> { new FileOrder { } } })
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

        [HttpPost]
        public async Task<GridPagedOutput<LogErrorDto>> ListLogError(GridPagedInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.SearchPhrase))
                    input.SearchPhrase = "";

                if (string.IsNullOrEmpty(input.Id))
                    throw new KeyNotFoundException($"DeviceIdentifier: {input.Id} not found.");

                var devices = await _setBoxApiAppService.GetDevices(new DeviceInput { DeviceIdentifier = input.Id });

                if (devices.Items.Count <= 0)
                    throw new KeyNotFoundException($"DeviceIdentifier: {input.Id} not found.");

                var logs = await _setBoxApiAppService.GetDeviceLogsErros(new LogInput
                {
                    DeviceIdentifier = input.Id,
                    Filter = input.SearchPhrase,
                    MaxResultCount = input.RowCount,
                    SkipCount = input.Current,
                    Sorting = string.Join(",", input.Sort.Select(x => x.Key + " " + x.Value).ToArray())
                });

                return new GridPagedOutput<LogErrorDto>(logs.Items) { Total = logs.TotalCount, RowCount = input.RowCount, Current = input.Current };
            }
            catch (Exception ex)
            {
                _logger.Error( ex.Message, ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<GridPagedOutput<LogAccessesDto>> ListLog(GridPagedInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.SearchPhrase))
                    input.SearchPhrase = "";

                if (string.IsNullOrEmpty(input.Id))
                    throw new KeyNotFoundException($"DeviceIdentifier: {input.Id} not found.");

                var devices = await _setBoxApiAppService.GetDevices(new DeviceInput { DeviceIdentifier = input.Id });

                if (devices.Items.Count <= 0)
                    throw new KeyNotFoundException($"DeviceIdentifier: {input.Id} not found.");

                var logs = await _setBoxApiAppService.GetDeviceLogsAccesses(new LogInput
                {
                    DeviceIdentifier = input.Id,
                    Filter = input.SearchPhrase,
                    MaxResultCount = input.RowCount,
                    SkipCount = input.Current,
                    Sorting = string.Join(",", input.Sort.Select(x => x.Key + " " + x.Value).ToArray())
                });

                return new GridPagedOutput<LogAccessesDto>(logs.Items) { Total = logs.TotalCount, RowCount = input.RowCount, Current = input.Current };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
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

        [HttpPost]
        public async Task<GridPagedOutput<DeviceDto>> ListDevices(GridPagedInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.SearchPhrase))
                    input.SearchPhrase = "";

                var logs = await _setBoxApiAppService.GetDevices(new DeviceInput
                {
                    DeviceIdentifier = input.Id,
                    Filter = input.SearchPhrase,
                    MaxResultCount = input.RowCount,
                    SkipCount = input.Current,
                    Sorting = string.Join(",", input.Sort.Select(x => x.Key + " " + x.Value).ToArray())
                });

                return new GridPagedOutput<DeviceDto>(logs.Items) { Total = logs.TotalCount, RowCount = input.RowCount, Current = input.Current };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
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
