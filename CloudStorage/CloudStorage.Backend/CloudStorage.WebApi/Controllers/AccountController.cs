using CloudStorage.Domain.ViewModels;
using CloudStorage.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.WebApi.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("registration")]
        public async Task<ActionResult> Registration([FromBody] RegistrationVm vm)
        {
            if(!ModelState.IsValid)
            {
                return ValidationProblem();
            }

            var dto = await _accountService.Registration(vm);

            if (dto.StatusCode == Domain.Enums.StatusCode.Ok)
            {
                return Ok(dto.Data);
            }
            return BadRequest();
        }
    }
}
