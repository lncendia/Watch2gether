using System.Security.Claims;
using Films.Application.Abstractions;

namespace Films.Infrastructure.Web.Authentication;

public static class Extensions
{
    public static Guid GetId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    public static string GetName(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Name)!;
    }

    public static Uri GetAvatar(this ClaimsPrincipal user)
    {
        return new Uri(user.FindFirstValue(ApplicationConstants.AvatarClaimType)!, UriKind.Relative);
    }

    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        return user.IsInRole(ApplicationConstants.AdminRoleName);
    }
}