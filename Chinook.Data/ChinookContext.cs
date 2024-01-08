using Chinook.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Data
{
    public class ChinookContext : DbContext
    {
        public ChinookContext(DbContextOptions<ChinookContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<NoteCategory> NoteCategories { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<AccessRightCategory> AccessRightCategories { get; set; }
        public DbSet<AccessRight> AccessRights { get; set; }
        public DbSet<AccessRightEndpoint> AccessRightEndpoints { get; set; }
        public DbSet<UserAccessRight> UserAccessRights { get; set; }

        public DbSet<MenuItemAccessRight> MenuItemAccessRights { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
