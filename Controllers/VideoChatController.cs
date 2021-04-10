using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sandbox.Services;
using SandboxBackend.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SandboxBackend.Controllers
{
    [Route("[controller]/[action]")]
    public class VideoChatController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly SandboxDbContext db;
        private IWebHostEnvironment env;

        public VideoChatController(
            UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManager,
            SandboxDbContext _db,
            IWebHostEnvironment _env)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            db = _db;
            env = _env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Json(new { view = await ServiceFunctions.RenderRazorViewToString(this, "Index") });
        }

        [HttpGet("{eid}/{vid}")]
        public IActionResult GetVideo(string eid, string vid)
        {
            string path = Path.Combine(env.ContentRootPath, $"Videos/{eid}/{userManager.GetUserId(User)}/{vid}E.MOV");
            Stream stream = System.IO.File.OpenRead(path);

            if (stream == null)
                return NotFound(); // returns a NotFoundResult with Status404NotFound response.

            return File(stream, "application/octet-stream", true); // returns a FileStreamResult
        }
    }
}
