using CloudStorage.Domain.Responses;
using CloudStorage.Domain.ViewModels;
using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Services.Interfaces
{
    public interface IFileService
    {
        Task<BaseResponse<IEnumerable<File>>> GetFiles(Guid id);
        Task<BaseResponse<bool>> AddDirectory(AddDirectoryVm vm, Guid userId);
        Task<BaseResponse<bool>> Upload(FileUploudVm vm, Guid userId);
    }
}
