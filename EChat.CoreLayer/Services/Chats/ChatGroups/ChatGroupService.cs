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
    public class ChatGroupService : BaseService, IChatGroupService
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

        public async Task<List<SearchResultViewModel>> Search(string searchText, long userId)
        {
            var result = new List<SearchResultViewModel>();
            if (string.IsNullOrWhiteSpace(searchText))
                return result;

            var groups = await Table<ChatGroup>()
                .Where(g => g.Title.Contains(searchText) && !g.IsPrivate)
                .Select(s => new SearchResultViewModel()
                {
                    ImageUrl = s.ImageUrl,
                    Token = s.Token,
                    IsUser = false,
                    Title = s.Title
                }).ToListAsync();

            var users = await Table<User>()
                .Where(g => g.UserName.Contains(searchText) && g.Id != userId)
                .Select(s => new SearchResultViewModel()
                {
                    ImageUrl = s.Avatar,
                    Token = s.Id.ToString(),
                    IsUser = true,
                    Title = s.UserName
                }).ToListAsync();
            result.AddRange(groups);
            result.AddRange(users);
            return result;
        }

        public async Task<ChatGroup> Get(long id)
        {
            return await Table<ChatGroup>()
                .Include(g => g.User)
                .Include(g => g.Receiver)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<ChatGroup> Get(string token)
        {
            return await Table<ChatGroup>()
                .Include(g => g.User)
                .Include(g => g.Receiver)
                .FirstOrDefaultAsync(g => g.Token == token);
        }

        public async Task<ChatGroup> CreatePrivateGroup(long userId, long receiverId)
        {
            var group = await Table<ChatGroup>()
                .Include(c => c.User)
                .Include(c => c.Receiver)
                .SingleOrDefaultAsync(s =>
                    (s.OwnerId == userId && s.ReceiverId == receiverId)
                    || (s.OwnerId == receiverId && s.ReceiverId == userId));

            if (group == null)
            {
                var groupCreated = new ChatGroup()
                {
                    CreateDate = DateTime.Now,
                    Title = $"Chat With {receiverId}",
                    Token = Guid.NewGuid().ToString(),
                    ImageUrl = "Default.jpg",
                    IsPrivate = true,
                    OwnerId = userId,
                    ReceiverId = receiverId
                };
                Insert(groupCreated);
                await Save();
                return await Get(groupCreated.Id);
            }
            return group;
        }
    }
}