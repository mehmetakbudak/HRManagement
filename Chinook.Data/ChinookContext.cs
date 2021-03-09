using Chinook.Model.Entities;
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
        public DbSet<Lookup> Lookups { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
