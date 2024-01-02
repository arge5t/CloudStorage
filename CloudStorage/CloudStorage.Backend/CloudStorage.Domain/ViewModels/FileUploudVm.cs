using Microsoft.AspNetCore.Http;

namespace CloudStorage.Domain.ViewModels
{
    public class FileUploudVm
    {
        public IFormFile File { get; set; }
        public Guid ParentId { get; set; }
    }
}
