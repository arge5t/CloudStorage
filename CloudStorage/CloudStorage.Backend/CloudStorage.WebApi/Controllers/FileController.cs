using CloudStorage.Domain.ViewModels;
using CloudStorage.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class FileController : BaseController
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService) => _fileService = fileService;

        [HttpGet("files")]
        public async Task<IActionResult> GetFiles(Guid id)
        {
            if (id == Guid.Empty) id = UserId;

            var response = await _fileService.GetFiles(id);

            if (response.StatusCode == Domain.Enums.StatusCode.Ok)
            {
                return Ok(response.Data != null ? response.Data : response.Message);
            }

            return BadRequest(response.Message);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddDirectory(AddDirectoryVm vm)
        {
            var response = await _fileService.AddDirectory(vm, UserId);

            if (response.StatusCode == Domain.Enums.StatusCode.Ok)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Message);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(FileUploudVm vm)
        {
            var response = await _fileService.Upload(vm, UserId);

            if (response.StatusCode == Domain.Enums.StatusCode.Ok)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Message);
        }
    }
}
