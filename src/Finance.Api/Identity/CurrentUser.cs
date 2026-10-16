using Finance.Application.Abstractions.Identity;
using System.Security.Claims;

namespace Finance.Api.Identity
{
    public class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
    {
        private readonly IHttpContextAccessor _accessor = accessor;

        public string UserId =>
            _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("No authenticated user in the current request.");
    }
}
