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
    public class UserGroupService : BaseService, IUserGroupService
    {
        public UserGroupService(EChatContext context) : base(context)
        {
        }

        public async Task<List<UserGroupViewModel>> GetAll(long userId)
        {
            var result = await Table<UserGroup>()
                .Include(g => g.ChatGroup.Chats)
                .Include(g => g.ChatGroup.Receiver)
                .Include(g => g.ChatGroup.User)
                .Where(g => g.UserId == userId).ToListAsync();

            var model = new List<UserGroupViewModel>();

            foreach (var ug in result)
            {
                var chatGroup = ug.ChatGroup;
                if (ug.ChatGroup.ReceiverId != null)
                {
                    if (chatGroup.ReceiverId == userId)
                        model.Add(new UserGroupViewModel()
                        {
                            GroupName = chatGroup.User.UserName,
                            ImageUrl = chatGroup.User.Avatar,
                            LastChat = chatGroup.Chats.OrderByDescending(c => c.CreateDate).FirstOrDefault(),
                            Token = chatGroup.Token
                        });
                    else
                        model.Add(new UserGroupViewModel()
                        {
                            GroupName = chatGroup.Receiver.UserName,
                            ImageUrl = chatGroup.Receiver.Avatar,
                            LastChat = chatGroup.Chats.OrderByDescending(c => c.CreateDate).FirstOrDefault(),
                            Token = chatGroup.Token
                        });
                }
                else
                {
                    model.Add(new UserGroupViewModel()
                    {
                        GroupName = chatGroup.Title,
                        ImageUrl = chatGroup.ImageUrl,
                        LastChat = chatGroup.Chats.OrderByDescending(c => c.CreateDate).FirstOrDefault(),
                        Token = chatGroup.Token
                    });
                }
            }

            return model;
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

        public async Task JoinGroup(List<long> userIDs, long groupId)
        {
            foreach (var userId in userIDs)
            {
                var model = new UserGroup()
                {
                    CreateDate = DateTime.Now,
                    GroupId = groupId,
                    UserId = userId
                };
            Insert(model);
            }
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