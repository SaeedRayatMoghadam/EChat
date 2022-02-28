using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EChat.CoreLayer.Services.Users.UserGroups;
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

        public async Task<ChatGroup> Create(string name, long userId)
        {
            var chatGroup = new ChatGroup()
            {
                Title = name,
                OwnerId = userId,
                Token = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now
            };

            Insert(chatGroup);

            await Save();

            await _userGroupService.JoinGroup(new UserGroup()
            {
                CreateDate = DateTime.Now,
                GroupId = chatGroup.Id,
                UserId = userId
            });

            return chatGroup;
        }
    }
}