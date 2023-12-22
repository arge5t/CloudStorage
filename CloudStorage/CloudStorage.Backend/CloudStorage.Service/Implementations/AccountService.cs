using CloudStorage.Domain.Responses;
using CloudStorage.Domain.ViewModels;
using CloudStorage.Services.Interfaces;
using CloudStorage.Persistence.Interfaces;
using CloudStorage.Domain.Entities;
using CloudStorage.Domain.Enums;
using CloudStorage.Domain.Dtos;

namespace CloudStorage.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepostory;
        private readonly IMailService _mailService;
        private readonly ITokenService _tokenService;

        private readonly CancellationToken _cancellationToken = new CancellationToken();

        private int _diskSpace = GigToBytes(8);

        public AccountService(
            IUserRepository userRepository,
            IMailService mailService,
            ITokenService tokenService)
        {
            _userRepostory = userRepository;
            _mailService = mailService;
            _tokenService = tokenService;
        }

        public async Task<BaseResponse<string>> Registration(RegistrationVm registrationVm)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = registrationVm.Name,
                Email = registrationVm.Email,
                Password = GetHashPassword(registrationVm.Password),
                isActivated = false,
                ActivationLink = Guid.NewGuid().ToString(),
                UsedSpace = 0,
                DiskSpace = _diskSpace,
                Avatar = string.Empty
            };

            try
            {
                var entity = await _userRepostory.GetUserByEmail(registrationVm.Email, _cancellationToken);

                if (entity != null)
                {
                    return new BaseResponse<string>()
                    {
                        Message = "A user with such an email has already been registered"
                    };
                }

                await _userRepostory.Create(user, _cancellationToken);

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
                    Message = $"Registration error: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<UserDto>> Login(LoginVm loginVm)
        {
            try
            {
                var user = await _userRepostory.GetUserByEmail(loginVm.Email, _cancellationToken);

                if (user == null || !PasswordVeryfied(loginVm.Password, user.Password))
                {
                    return new BaseResponse<UserDto>()
                    {
                        Message = "Incorrect username or password"
                    };
                }

                if (!user.isActivated)
                {
                    return new BaseResponse<UserDto>()
                    {
                        Message = "Email not confirm"
                    };
                }

                string jwt = _tokenService.GenerateToken(user.Id.ToString(), user.Name, user.Email);

                var refreshToken = await _tokenService.Create(user.Id, jwt);

                return new BaseResponse<UserDto>()
                {
                    StatusCode = StatusCode.Ok,
                    Data = new UserDto()
                    {
                        Name = user.Name,
                        Email = user.Email,
                        AccessToken = jwt,
                        RefreshToken = refreshToken.ToString()
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserDto>()
                {
                    Message = $"Registtration error: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<bool>> ActivateEmail(string activationLink)
        {
            try
                {
                var user = await _userRepostory.GetUserByActivationLink(activationLink, _cancellationToken);

                if (user == null || user.isActivated)
                {
                    return new BaseResponse<bool>()
                    {
                        Message = "User not found",
                        Data = false
                    };
                }

                user.isActivated = true;

                await _userRepostory.Update(user, _cancellationToken);

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.Ok,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Message = $"Registtration error: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<UserDto>> Refresh(string userEmail, Guid refreshToken)
        {
            try
            {
                var user = await _userRepostory.GetUserByEmail(userEmail, _cancellationToken);

                if (user == null)
                {
                    return new BaseResponse<UserDto>()
                    {
                        Message = "User not found"
                    };
                }

                string jwt = _tokenService.GenerateToken(user.Id.ToString(), user.Name, user.Email);

                var tokenId = await _tokenService.Edit(user.Id, jwt);

                return new BaseResponse<UserDto>()
                {
                    StatusCode = StatusCode.Ok,
                    Data = new UserDto()
                    {
                        Name = user.Name,
                        Email = user.Email,
                        AccessToken = jwt,
                        RefreshToken = tokenId.ToString()
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserDto>()
                {
                    Message = $"Token refresh error: {ex.Message}",
                };
            }
        }

        private string GetHashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(4));

        private bool PasswordVeryfied(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);

        private static int GigToBytes(int gigabytesNum) => gigabytesNum * 1024 * 1024;
    }
}
