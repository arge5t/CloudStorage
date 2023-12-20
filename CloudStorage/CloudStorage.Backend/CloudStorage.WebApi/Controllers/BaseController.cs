using CloudStorage.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CloudStorage.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
        internal Guid UserId => !User.Identity.IsAuthenticated
            ? Guid.Empty : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}
