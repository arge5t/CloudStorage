using CloudStorage.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Persistence.Repositories
{
    internal class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FileRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task Create(File file)
        {
            await _dbContext.Files.AddAsync(file);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(File file)
        {
            _dbContext.Files.Remove(file);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<File>> GetFiles(Guid id)
        {
            var files = await _dbContext.Files.Where(file => file.ParentId == id).ToListAsync();
            return files;
        }

        public async Task<File> GetParent(Guid parentId)
        {
            var parent = await _dbContext.Files.FirstOrDefaultAsync(file => file.Id == parentId);

            return parent;
        }
    }
}
