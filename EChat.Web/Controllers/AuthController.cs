using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EChat.CoreLayer.Services.Users;
using EChat.CoreLayer.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace EChat.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var result = await _userService.Register(model);

            if (!result)
            {
                ModelState.AddModelError(model.UserName, "UserName is NOT available .");
                return View("Index", model);
            }

            return Redirect("/Auth");
        }

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var user = await _userService.Login(model);

            if (user == null)
            {
                ModelState.AddModelError(model.UserName, "Invalid Inputs .");
                return View("Index", model);
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principals = new ClaimsPrincipal(claimIdentity);
            var properties = new AuthenticationProperties()
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(principals);

            return Redirect("/");
        }
    }
}
