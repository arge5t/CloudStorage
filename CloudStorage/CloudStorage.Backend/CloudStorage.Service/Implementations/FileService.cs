using CloudStorage.Domain.Responses;
using CloudStorage.Domain.ViewModels;
using CloudStorage.Persistence.Interfaces;
using CloudStorage.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Services.Implementations
{
    internal class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;

        private CancellationToken _cancellationToken = new();

        private readonly string _storePath;

        public FileService(IFileRepository fileRepository,
            IUserRepository userRepository,
            string storePath)
        {
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _storePath = $"{storePath}\\Store";
        }

        public async Task<BaseResponse<bool>> AddDirectory(AddDirectoryVm vm, Guid userId)
        {
            try
            {
                var fileParent = await _fileRepository.GetParent(vm.ParentId);

                var file = new File()
                {
                    Id = Guid.NewGuid(),
                    Name = vm.Name,
                    Type = vm.Type,
                    CreateTime = DateTime.Now,
                    EditTime = null,
                    UserId = userId
                };

                if (fileParent != null)
                {
                    file.ParentId = fileParent.Id;
                    file.Path = $"{fileParent.Path}\\{file.Name}";
                }
                else
                {
                    file.ParentId = userId;
                    file.Path = vm.Name;
                }

                CreateFolder($"{_storePath}\\{userId}\\{file.Path}");

                await _fileRepository.Create(file);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = Domain.Enums.StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    Message = $"Add Folder error: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<File>>> GetFiles(Guid id)
        {
            try
            {
                var files = await _fileRepository.GetFiles(id);

                if (files.Count() == 0)
                {
                    return new BaseResponse<IEnumerable<File>>()
                    {
                        StatusCode = Domain.Enums.StatusCode.Ok,
                        Message = "It's empty here so far, add your first file"
                    };
                }

                return new BaseResponse<IEnumerable<File>>()
                {
                    StatusCode = Domain.Enums.StatusCode.Ok,
                    Data = files
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<File>>()
                {
                    Message = $"Error GetFiles: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<bool>> Upload(FileUploudVm vm, Guid userId)
        {
            try
            {
                var fileParent = await _fileRepository.GetParent(vm.ParentId);

                var user = await _userRepository.GetUserById(userId, _cancellationToken);


                if (user.UsedSpace + vm.File.Length > user.DiskSpace)
                {
                    throw new Exception("There no space on the disk");
                }

                user.UsedSpace += vm.File.Length;

                var file = vm.File;

                var entity = new File()
                {
                    Id = Guid.NewGuid(),
                    Name = file.FileName,
                    Type = file.FileName.Split(".").Last(),
                    Size = file.Length,
                    CreateTime = DateTime.Now,
                    EditTime = null,
                    ParentId = fileParent.Id,
                    UserId = userId
                };

               

                entity.Path = fileParent != null 
                    ?
                        $"{fileParent.Path}\\{vm.File.FileName}"
                    :
                        $"{vm.File.FileName}";

                var path = $"{_storePath}\\{userId}\\{entity.Path}";

                await UploadFile(vm.File, path);

                await _fileRepository.Create(entity);
                await _userRepository.Update(user, _cancellationToken);

                return new BaseResponse<bool>()
                {
                    StatusCode = Domain.Enums.StatusCode.Ok,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Message = $"Error Upload: {ex.Message}"
                };
            }
        }

        private void CreateFolder(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                throw new Exception("such a file already exists");
            }
        }

        private async Task UploadFile(IFormFile uploadedFile, string path)
        {
            if (path == null || uploadedFile == null) throw new ArgumentNullException();

            if (!Directory.Exists(path))
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }
            else
            {
                throw new Exception("such a file already exists");
            }
        }
    }
}
