using System.Security.Claims;

namespace Films.Infrastructure.Web.Authentication;

public static class Extensions
{
    public static Guid GetId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}