using Chinook.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Data
{
    public class ChinookContext : DbContext
    {
        public ChinookContext(DbContextOptions<ChinookContext> options) : base(options) { }

        public DbSet<AccessRightCategoryDmo> AccessRightCategories { get; set; }
        public DbSet<AccessRightDmo> AccessRights { get; set; }
        public DbSet<AccessRightEndpointDmo> AccessRightEndpoints { get; set; }
        public DbSet<BlogCategoryDmo> BlogCategories { get; set; }
        public DbSet<BlogDmo> Blogs { get; set; }
        public DbSet<CityDmo> Cities { get; set; }
        public DbSet<CommentDmo> Comments { get; set; }
        public DbSet<MenuDmo> Menus { get; set; }
        public DbSet<MenuItemAccessRightDmo> MenuItemAccessRights { get; set; }
        public DbSet<MenuItemDmo> MenuItems { get; set; }
        public DbSet<PageDmo> Pages { get; set; }
        public DbSet<ProvinceDmo> Provinces { get; set; }
        public DbSet<RoleAccessRightDmo> RoleAccessRights { get; set; }
        public DbSet<RoleDmo> Roles { get; set; }
        public DbSet<SelectedBlogCategoryDmo> SelectedBlogCategories { get; set; }
        public DbSet<SourceTagDmo> SourceTags { get; set; }
        public DbSet<TagDmo> Tags { get; set; }
        public DbSet<TaskCategoryDmo> TaskCategories { get; set; }
        public DbSet<TaskDmo> Tasks { get; set; }
        public DbSet<TaskStatusDmo> TaskStatuses { get; set; }
        public DbSet<TitleDmo> Titles { get; set; }
        public DbSet<UserAccessRightDmo> UserAccessRights { get; set; }
        public DbSet<UserDmo> Users { get; set; }
        public DbSet<UserRoleDmo> UserRoles { get; set; }
        public DbSet<UserTokenDmo> UserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
