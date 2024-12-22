using CloudStorage.Domain.ViewModels;
using CloudStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
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

        [AllowAnonymous]
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

                SaveTokenToCookies(data.Email, data.RefreshToken);
                return Ok(dto.Data);
            }
            return BadRequest(dto.Message);
        }

        [AllowAnonymous]
        [HttpGet("activate/{link}")]
        public async Task<ActionResult> Activate(string link)
        {
            var response = await _accountService.ActivateEmail(link);

            if (response.StatusCode == Domain.Enums.StatusCode.Ok && response.Data)
            {
                return Ok(link);
            }
            return BadRequest(response.Message);
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            string userName = GetCookie("X-Username");
            string refreshToken = GetCookie("X-Refresh-Token");

            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(refreshToken)) return BadRequest();

            var dto = await _accountService.Refresh(userName, new Guid(refreshToken));

            if (dto.StatusCode == Domain.Enums.StatusCode.Ok)
            {
                var data = dto.Data;

                SaveTokenToCookies(data.Email, data.RefreshToken);
                return Ok(data);
            }

            return BadRequest(dto.Message);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            string refreshToken = GetCookie("X-Refresh-Token");

            if (string.IsNullOrEmpty(refreshToken)) return BadRequest();

            var response = await _accountService.Logout(new Guid(refreshToken), UserId);

            bool isLogout = response.Data;

            if (isLogout)
            {
                DeleteCookies();
                return Ok(isLogout);
            }

            return BadRequest(response.Message);
        }

        private string GetCookie(string key)
        {
            var cookieValue = HttpContext.Request.Cookies[key];

            if (!string.IsNullOrWhiteSpace(cookieValue))
            {
                return cookieValue;
            }

            return string.Empty;
        }

        private void SaveTokenToCookies(string userEmail, string refresh)
        {
            var expires = DateTimeOffset.Now.AddDays(5);

            Response.Cookies.Append("X-Username", userEmail, new CookieOptions() { Expires = expires });
            Response.Cookies.Append("X-Refresh-Token", refresh, new CookieOptions() { Expires = expires});
        }

        private void DeleteCookies()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
        }
    }
}
