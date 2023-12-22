using CloudStorage.Domain.Dtos;
using CloudStorage.Domain.Responses;
using CloudStorage.Domain.ViewModels;

namespace CloudStorage.Services.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<string>> Registration(RegistrationVm registrationVm);
        Task<BaseResponse<UserDto>> Login(LoginVm loginVm);
        Task<BaseResponse<bool>> ActivateEmail(string activationLink);
        Task<BaseResponse<UserDto>> Refresh(string userEmail, Guid refreshToken);
    }
}
