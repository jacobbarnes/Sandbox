using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SandboxBackend.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandboxBackend.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly SandboxDbContext db;

        public DashboardController(
            UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManager,
            SandboxDbContext _db)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            db = _db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetChat()
        {
            return Json(new { view="Chat" });
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetProfile()
        {
            return Json(new { view = "Profile <button onclick='logout()'>Logout</button>" });
        }
    }
}
