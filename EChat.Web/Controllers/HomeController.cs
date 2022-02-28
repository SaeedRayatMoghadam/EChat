using EChat.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EChat.CoreLayer.Services.Chats.ChatGroups;
using EChat.CoreLayer.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace EChat.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IChatGroupService _chatGroupService;

        public HomeController(ILogger<HomeController> logger, IChatGroupService chatGroupService)
        {
            _logger = logger;
            _chatGroupService = chatGroupService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model = await _chatGroupService.GetAll(User.GetUserId());
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
