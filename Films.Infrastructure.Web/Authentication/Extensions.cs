using System.Security.Claims;
using IdentityModel;

namespace Films.Infrastructure.Web.Authentication;

public static class Extensions
{
    public static Guid GetId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue(JwtClaimTypes.Subject)!);
    }
}