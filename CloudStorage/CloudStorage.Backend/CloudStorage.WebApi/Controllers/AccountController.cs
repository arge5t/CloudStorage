using Azure;
using CloudStorage.Domain.ViewModels;
using CloudStorage.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.WebApi.Controllers
{
    [Route("api/[controller]")]
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
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }

            var dto = await _accountService.Registration(vm);

            if (dto.StatusCode == Domain.Enums.StatusCode.Ok)
            {
                return Ok(dto.Data);
            }
            return BadRequest(dto.Message);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginVm vm)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }

            var dto = await _accountService.Login(vm);

            if (dto.StatusCode == Domain.Enums.StatusCode.Ok)
            {
                var data = dto.Data;

                SaveTokenToCookies(data.AccessToken, data.Email, data.RefreshToken);
                return Ok(dto.Data);
            }
            return BadRequest(dto.Message);
        }

        [HttpGet("activate/{link}")]
        public async Task<ActionResult> Activate(string link)
        {
            var response = await _accountService.ActivateEmail(link);

            if (response.StatusCode == Domain.Enums.StatusCode.Ok && response.Data)
            {
                return Redirect("https://localhost:7081/swagger/index.html");
            }
            return BadRequest(response.Message);
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!(Request.Cookies.TryGetValue("X-Username", out var userName) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
                return BadRequest();

            var dto = await _accountService.Refresh(userName, new Guid(refreshToken));

            if (dto.StatusCode == Domain.Enums.StatusCode.Ok)
            {
                var data = dto.Data;

                SaveTokenToCookies(data.AccessToken, data.Email, data.RefreshToken);
                return Ok(data);
            }

            return BadRequest(dto.Message);
        }


        private void SaveTokenToCookies(string token, string userEmail, string refresh)
        {
            Response.Cookies.Append("X-Access-Token", token, new CookieOptions());
            Response.Cookies.Append("X-Username", userEmail, new CookieOptions());
            Response.Cookies.Append("X-Refresh-Token", refresh, new CookieOptions());
        }
    }
}
