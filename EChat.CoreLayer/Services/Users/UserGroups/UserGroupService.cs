using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EChat.CoreLayer.ViewModels.Chats;
using EChat.DataLayer.Context;
using EChat.DataLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace EChat.CoreLayer.Services.Users.UserGroups
{
    public class UserGroupService : BaseService,IUserGroupService
    {
        public UserGroupService(EChatContext context) : base(context)
        {
        }

        public async Task<List<UserGroupViewModel>> GetAll(long userId)
        {
            var result = Table<UserGroup>()
                .Include(g => g.ChatGroup.Chats)
                .Where(g => g.UserId == userId)
                .Select(g => new UserGroupViewModel()
                {
                    GroupName = g.ChatGroup.Title,
                    ImageUrl = g.ChatGroup.ImageUrl,
                    LastChat = g.ChatGroup.Chats.OrderBy(c => c.CreateDate).First(),
                    Token = g.ChatGroup.Token
                });

            return await result.ToListAsync();
        }

        public async Task JoinGroup(long userId, long groupId)
        {
            var model = new UserGroup()
            {
                CreateDate = DateTime.Now,
                GroupId = groupId,
                UserId = userId
            };
            Insert(model);
            await Save();
        }

        public async Task<bool> IsUserInGroup(long userId, long groupId)
        {
            return await Table<UserGroup>().AnyAsync(g => g.UserId == userId && g.GroupId == groupId);
        }

        public async Task<bool> IsUserInGroup(long userId, string token)
        {
            return await Table<UserGroup>()
                .Include(g => g.ChatGroup)
                .AnyAsync(g => g.UserId == userId && g.ChatGroup.Token == token);
        }
    }
}