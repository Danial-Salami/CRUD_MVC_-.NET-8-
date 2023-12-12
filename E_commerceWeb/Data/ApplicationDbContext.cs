using E_commerceWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace E_commerceWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "phone", DisplayOrder = 1 },
                new Category { Id = 2, Name = "computer", DisplayOrder = 2 },
                new Category { Id = 3, Name = "tablet", DisplayOrder = 3 }
                );
        }
    }
}
