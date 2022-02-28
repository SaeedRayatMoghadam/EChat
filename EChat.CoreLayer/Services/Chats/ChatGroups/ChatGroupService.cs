using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EChat.DataLayer.Context;
using EChat.DataLayer.Entities.Chats;
using Microsoft.EntityFrameworkCore;

namespace EChat.CoreLayer.Services.Chats.ChatGroups
{
    public class ChatGroupService : BaseService,IChatGroupService
    {
        public ChatGroupService(EChatContext context) : base(context)
        {
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

            return chatGroup;
        }
    }
}