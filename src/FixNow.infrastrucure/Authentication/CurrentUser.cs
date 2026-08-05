using System.Security.Claims;

using Microsoft.AspNetCore.Http;

namespace FixNow.Infrastructure.Authentication;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private const string SubClaimType = "sub";

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public Guid UserId
    {
        get
        {
            if (!IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userIdClaim =
                _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? _httpContextAccessor.HttpContext.User.FindFirstValue(SubClaimType);

            if (string.IsNullOrWhiteSpace(userIdClaim)
                || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User identifier claim is missing or invalid.");
            }

            return userId;
        }
    }
}
