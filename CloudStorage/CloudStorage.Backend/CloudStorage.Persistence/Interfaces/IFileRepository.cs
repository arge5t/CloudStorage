using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Persistence.Interfaces
{
    public interface IFileRepository
    {
        Task Create(File file);
        Task Delete(File file);
        Task<IEnumerable<File>> GetFiles(Guid id);
        Task<File> GetParent(Guid parentId);
    }
}
