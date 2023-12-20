using CloudStorage.Domain.Entities;
using CloudStorage.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
