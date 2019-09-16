using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SupportController : BaseController
    {
        private readonly ILogger<SupportController> _logger;
        private readonly IRepository<Support, Guid> _support;

        /// <summary>
        /// SetBoxController
        /// </summary>
        public SupportController(ILogger<SupportController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _support = new Repository<Support, Guid>(context);

        }

        public async Task<IActionResult> Index()
        {
            var support = await _support.FirstOrDefaultAsync();

            if (support == null)
                return View(new SupportViewModel("") { SupportId = Guid.NewGuid() });

            return View(new SupportViewModel("")
            {
                SupportId = support.SupportId,
                Company = support.Company,
                Name = support.Name,
                Email = support.Email,
                Telephone = support.Telephone
            });


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SupportViewModel s)
        {
            s.Title = "";
            s.Mensage = "";

            if (ModelState.IsValid) //verifica se é válido
            {
                try
                {
                    var support = await _support.FirstOrDefaultAsync();
                    bool isNew = false;

                    if (support == null)
                    {
                        isNew = true;
                        support = new Support
                        {
                            SupportId = Guid.NewGuid()
                        };
                    }

                    support.Company = s.Company;
                    support.Email = s.Email;
                    support.Name = s.Name;
                    support.Telephone = s.Telephone;
                    support.CreationDateTime = DateTime.Now;

                    if (isNew)
                        await _support.AddAsync(support);
                    else
                        await _support.UpdateAsync(support);

                    s.SupportId = support.SupportId;

                    s.Title = "Success";
                    s.Mensage = "Update was successful.";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(s);
        }
    }
}