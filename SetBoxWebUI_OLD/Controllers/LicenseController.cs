using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SetBoxWebUI.Helpers;
using SetBoxWebUI.Interfaces;
using SetBoxWebUI.Models;
using SetBoxWebUI.Models.Views;
using SetBoxWebUI.Repository;

namespace SetBoxWebUI.Controllers
{
    public class LicenseController : Controller
    {

        private readonly ILogger<LicenseController> _logger;
        private readonly IRepository<Device, Guid> _devices;

        /// <summary>
        /// SetBoxController
        /// </summary>
        public LicenseController(ILogger<LicenseController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _devices = new Repository<Device, Guid>(context);
        }

        public async Task<IActionResult> Index(string deviceIdentifier = null)
        {
            try
            {
                LicenseViewModel model = new LicenseViewModel()
                {
                    deviceIdentifier = deviceIdentifier
                };
                var devices = await _devices.FirstOrDefaultAsync(f => f.DeviceIdentifier == deviceIdentifier);
                if (devices != null)
                {
                    model.DeviceId = devices.DeviceId;
                    model.Session = CriptoHelpers.Base64Encode(devices.DeviceId.ToString());
                    return View(model);
                }
                else
                {
                    if (!string.IsNullOrEmpty(deviceIdentifier))
                    {
                        _logger.LogDebug($"deviceIdentifier {deviceIdentifier} invalid!");
                        return View(new LicenseViewModel($"Device {deviceIdentifier} não cadastro!") { deviceIdentifier = deviceIdentifier });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View(new LicenseViewModel(ex));
            }
            return View(new LicenseViewModel());
        }

        public async Task<string> Key(string identifier, string session)
        {
            var deviceId = CriptoHelpers.Base64Decode(session);
            var devices = await _devices.FirstOrDefaultAsync(f => f.DeviceId.ToString() == deviceId);
            if (devices != null)
            {
                devices.License = CriptoHelpers.MD5HashString(identifier);
                await _devices.UpdateAsync(devices);
                return devices.License;
            }
            return "";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LicenseViewModel model)
        {
            try
            {
                var devices = await _devices.FirstOrDefaultAsync(f => f.DeviceIdentifier == model.deviceIdentifier);
                if (devices != null)
                {
                    model.DeviceId = devices.DeviceId;
                    model.Session = CriptoHelpers.Base64Encode(devices.DeviceId.ToString());
                    model.deviceLicense = await Key(model.deviceIdentifier, model.Session);
                    return View(model);
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.deviceIdentifier))
                    {
                        _logger.LogDebug($"deviceIdentifier {model.deviceIdentifier} invalid!");
                        return View(new LicenseViewModel($"Device {model.deviceIdentifier} não cadastro!") { deviceIdentifier = model.deviceIdentifier });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View(new LicenseViewModel(ex));
            }
            return View(new LicenseViewModel());
        }
    }
}