using Microsoft.EntityFrameworkCore;
using IeltsPlatform.ApiService.Entities;
using Microsoft.EntityFrameworkCore.Internal;
using System.Net.Sockets;

namespace IeltsPlatform.ApiService.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {

        }

        public DbSet<Blog> Blogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        
    }
}