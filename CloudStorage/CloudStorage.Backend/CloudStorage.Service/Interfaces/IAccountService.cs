using CloudStorage.Domain.Dtos;
using CloudStorage.Domain.Responses;
using CloudStorage.Domain.ViewModels;

namespace CloudStorage.Services.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<string>> Registration(RegistrationVm userVm);
    }
}
