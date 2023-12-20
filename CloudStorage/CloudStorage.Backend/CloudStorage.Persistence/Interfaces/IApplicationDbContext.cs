using CloudStorage.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Persistence.Interfaces
{
    public interface IApplicationDbContext : IBaseDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<File> Files { get; set; }
        DbSet<Token> Tokens { get; set; }
    }
}
