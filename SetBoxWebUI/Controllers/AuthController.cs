using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Afonsoft.EFCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SetBoxWebUI.Helpers;
using SetBoxWebUI.Models;
using SetBoxWebUI.Repository;

namespace SetBoxWebUI.Controllers
{
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly SignInManager<ApplicationIdentityUser> _signInManager;
        public const string SessionKeyId = "_UserId";

        public AuthController(ILogger<AuthController> logger, SignInManager<ApplicationIdentityUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public ActionResult Denied()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Auth");
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl = "")
        {
            var model = new LoginModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel u)
        {
            if (ModelState.IsValid) //verifica se é válido
            {

                var result = await _signInManager.PasswordSignInAsync(u.Username, u.Password, true, true);

                if (result.Succeeded)
                {

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                    };
                    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, User, authProperties);

                    if (!string.IsNullOrEmpty(u.ReturnUrl) && Url.IsLocalUrl(u.ReturnUrl))
                    {
                        return Redirect(u.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("User", "Usuário ou Senha Inválidos!");
                }
            }
            return View(u);
        }

        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}