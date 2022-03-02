using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EChat.CoreLayer.Utilities.Security;
using EChat.CoreLayer.ViewModels;
using EChat.DataLayer.Context;
using EChat.DataLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace EChat.CoreLayer.Services.Users
{
    public class UserService : BaseService,IUserService
    {
        public UserService(EChatContext context) : base(context)
        {
        }

        public async Task<bool> IsUserExist(string userName)
        {
            return await Table<User>().AnyAsync(u => u.UserName == userName);
        }

        public async Task<bool> IsUserExist(long id)
        {
            return await Table<User>().AnyAsync(u => u.Id == id);
        }

        public async Task<bool> Register(RegisterViewModel model)
        {
            if (await IsUserExist(model.UserName))
                return false;

            if (model.Password != model.ConfirmPassword)
                return false;
            
            var pass = model.Password.EncodePasswordMd5();
            var user = new User()
            {
                Avatar = "Default.jpg",
                CreateDate = DateTime.Now,
                Password = pass,
                UserName = model.UserName.ToLower()
            };

            Insert(user);
            await Save();

            return true;
        }

        public async Task<User> Login(LoginViewModel model)
        {
            var user = await Table<User>().SingleOrDefaultAsync(u => u.UserName == model.UserName.ToLower());

            if (user == null)
                return null;

            var pass = model.Password.EncodePasswordMd5();

            if (user.Password != pass)
                return null;

            return user;
        }

        public async Task<List<string>> GetUsersIDs(long groupId)
        {
            return await Table<UserGroup>()
                .Where(ug => ug.GroupId == groupId)
                .Select(u => u.UserId.ToString())
                .ToListAsync();
        }
    }
}