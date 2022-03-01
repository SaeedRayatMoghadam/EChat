using EChat.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EChat.CoreLayer.Services.Chats.ChatGroups;
using EChat.CoreLayer.Services.Users.UserGroups;
using EChat.CoreLayer.Utilities;
using EChat.CoreLayer.ViewModels.Chats;
using EChat.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EChat.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IChatGroupService _chatGroupService;
        private IHubContext<ChatHub> _chatHub;
        private readonly IUserGroupService _userGroupService;

        public HomeController(IChatGroupService chatGroupService, IHubContext<ChatHub> chatHub, IUserGroupService userGroupService)
        {
            _chatGroupService = chatGroupService;
            _chatHub = chatHub;
            _userGroupService = userGroupService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model = await _userGroupService.GetAll(User.GetUserId());
            return View(model);
        }
        
        [Authorize]
        [HttpPost]
        public async Task CreateGroup([FromForm]CreateGroupViewModel model)
        {
            try
            {
                model.UserId = User.GetUserId();
                var result = await _chatGroupService.Create(model);
                _chatHub.Clients.User(User.GetUserId().ToString()).SendAsync("NewGroup",result.Title, result.Token, result.ImageUrl);
            }
            catch
            {
                _chatHub.Clients.User(User.GetUserId().ToString()).SendAsync("NewGroup", "ERROR");
            }
        }

        public async Task<IActionResult> Search(string search)
        {
            return new ObjectResult(await _chatGroupService.Search(search));
        }
    }
}
