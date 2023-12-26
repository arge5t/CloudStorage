using Azure;
using CloudStorage.Domain.ViewModels;
using CloudStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

                SaveTokenToCookies(data.AccessToken, data.Email, data.RefreshToken);
                return Ok(data);
            }

            return BadRequest(dto.Message);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

            return BadRequest();
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

        private void SaveTokenToCookies(string token, string userEmail, string refresh)
        {
            Response.Cookies.Append("X-Access-Token", token, new CookieOptions());
            Response.Cookies.Append("X-Username", userEmail, new CookieOptions());
            Response.Cookies.Append("X-Refresh-Token", refresh, new CookieOptions());
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
