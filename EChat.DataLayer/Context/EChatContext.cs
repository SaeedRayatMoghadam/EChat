using System.Linq;
using EChat.DataLayer.Entities.Chats;
using EChat.DataLayer.Entities.Roles;
using EChat.DataLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace EChat.DataLayer.Context
{
    public class EChatContext : DbContext
    {
        public EChatContext(DbContextOptions<EChatContext> options):base(options)
        {
            
        }

        #region Entities

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var cascades = modelBuilder.Model.GetEntityTypes()
            //    .SelectMany(e => e.GetForeignKeys())
            //    .Where(fk => fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            //foreach (var fk in cascades)
            //{
            //    fk.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.User)
                .WithMany(c => c.Chats)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserGroup>()
                .HasOne(c => c.User)
                .WithMany(c => c.UserGroups)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            

            base.OnModelCreating(modelBuilder);
        }
    }
}