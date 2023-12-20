using CloudStorage.Domain.Responses;
using CloudStorage.Domain.ViewModels;
using CloudStorage.Services.Interfaces;
using CloudStorage.Persistence.Interfaces;
using CloudStorage.Domain.Entities;
using CloudStorage.Domain.Enums;

namespace CloudStorage.Services.Implementations
{
    internal class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepostory;
        private readonly IMailService _mailService;

        private readonly CancellationToken cancellationToken = new CancellationToken();

        private int _diskSpace = GigToBytes(8);

        public AccountService(
            IUserRepository userRepository, IMailService tokenService)
        {
            _userRepostory = userRepository;
            _mailService = tokenService;

        }

        public async Task<BaseResponse<string>> Registration(RegistrationVm userVm)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = userVm.Name,
                Email = userVm.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userVm.Password, 4),
                isActivated = false,
                ActivationLink = Guid.NewGuid().ToString(),
                UsedSpace = 0,
                DiskSpace = _diskSpace,
                Avatar = string.Empty
            };

            try
            {
                await _userRepostory.Create(user, cancellationToken);

                await _mailService.SendEmailConfirm(user.ActivationLink, user.Email);

                return new BaseResponse<string>()
                {
                    StatusCode = StatusCode.Ok,
                    Data = "Go to your email and activate your account"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>()
                {
                    Message = $"Registtration error: {ex.Message}",
                };
            }
        }

        private static int GigToBytes(int gigabytesNum) => gigabytesNum * 1024 * 1024;
    }
}
