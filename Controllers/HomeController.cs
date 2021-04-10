using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SandboxBackend.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SandboxBackend.DAL;
using Sandbox.Services;
using Microsoft.AspNetCore.Authorization;

namespace SandboxBackend.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly SandboxDbContext db;

        public HomeController(
            UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManager,
            SandboxDbContext _db)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            db = _db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return PartialView("App");
        }

        public async Task<IActionResult> LoadContent()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Json(new { view = await ServiceFunctions.RenderRazorViewToString(this, "Dashboard") });
            }
            return Json(new { view = await ServiceFunctions.RenderRazorViewToString(this, "Login") });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<ActionResult> Register(string Email, string Password)
        {
            string[] errors;
            var user = new IdentityUser() { UserName = Email };

            IdentityResult result = new();
            try
            {
                Password ??= "";
                result = await userManager.CreateAsync(user, Password);
            }
            catch (Exception e)
            {

            }

            if (result.Succeeded)
            {
                return Json(new { success = true });

            }
            else
            {
                errors = result.Errors.Select(error => error.Description).ToArray();
                string errorMessages = String.Join("\n", errors);
                return Json(new { success = false, errorMessages });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Login(string Email, string Password,
            bool rememberMe)
        {
            IdentityUser user = null;
            
            Email ??= "";
            Password ??= "";
            user = await userManager.FindByNameAsync(Email);
            if (!await userManager.CheckPasswordAsync(user, Password))
            {
                user = null;
            }
            
            if (user != null)
            {
                await signInManager.SignInAsync(user, rememberMe);
                return RedirectToAction("LoadContent");
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public bool NotAuthorized()
        {
            return false;
        }

        [AllowAnonymous]
        public async Task<ActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return Json(new { view = await ServiceFunctions.RenderRazorViewToString(this, "Login") });
        }
    }
}
