using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EChat.CoreLayer.Services.Users.UserGroups;
using EChat.CoreLayer.Utilities;
using EChat.CoreLayer.ViewModels.Chats;
using EChat.DataLayer.Context;
using EChat.DataLayer.Entities.Chats;
using EChat.DataLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace EChat.CoreLayer.Services.Chats.ChatGroups
{
    public class ChatGroupService : BaseService,IChatGroupService
    {
        private readonly IUserGroupService _userGroupService;
        public ChatGroupService(EChatContext context, IUserGroupService userGroupService) : base(context)
        {
            _userGroupService = userGroupService;
        }

        public async Task<List<ChatGroup>> GetAll(long userId)
        {
            return await Table<ChatGroup>()
                .Include(g => g.Chats)
                .Where(g => g.OwnerId == userId)
                .OrderByDescending(g => g.CreateDate)
                .ToListAsync();
        }

        public async Task<ChatGroup> Create(CreateGroupViewModel model)
        {
            if (model.ImageFile == null || !FileValidation.IsValidImageFile(model.ImageFile.FileName))
                throw new Exception();
            

            var imageName = await model.ImageFile.SaveFile("wwwroot/images/groups");


            var chatGroup = new ChatGroup()
            {
                Title = model.GroupName,
                OwnerId = model.UserId,
                Token = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now,
                ImageUrl = imageName
            };

            Insert(chatGroup);

            await Save();

            await _userGroupService.JoinGroup(model.UserId, chatGroup.Id);

            return chatGroup;
        }

        public async Task<List<SearchResultViewModel>> Search(string searchText)
        {
            var result = new List<SearchResultViewModel>();

            if (string.IsNullOrEmpty(searchText))
                return result;

            var groups = await Table<ChatGroup>()
                .Where(g => g.Title.Contains(searchText))
                .Select(g => new SearchResultViewModel()
                {
                    ImageUrl = g.ImageUrl,
                    Token = g.Token,
                    IsUser = false,
                    Title = g.Title
                }).ToListAsync();

            var users = await Table<User>()
                .Where(u => u.UserName.Contains(searchText))
                .Select(u => new SearchResultViewModel()
                {
                    ImageUrl = u.Avatar,
                    Token = u.Id.ToString(),
                    IsUser = true,
                    Title = u.UserName
                }).ToListAsync();

            result.AddRange(groups);
            result.AddRange(users);

            return result;
        }

        public async Task<ChatGroup> Get(long id)
        {
            return await GetById<ChatGroup>(id);
        }

        public async Task<ChatGroup> Get(string token)
        {
            return await Table<ChatGroup>()
                .Include(g => g.Chats)
                .FirstOrDefaultAsync(g => g.Token == token);
        }
    }
}