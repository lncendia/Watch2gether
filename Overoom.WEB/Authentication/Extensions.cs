using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Overoom.Application.Abstractions;
using Overoom.WEB.RoomAuthentication;

namespace Overoom.WEB.Authentication;

public static class Extensions
{
    public static async Task SignInRoomAsync(this HttpContext context, int id, Guid roomId, RoomType type)
    {
        await context.SignInAsync(ApplicationConstants.RoomScheme,
            new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim("RoomId", roomId.ToString()),
                new Claim("RoomType", type.ToString())
            }, ApplicationConstants.RoomScheme)), new AuthenticationProperties { IsPersistent = true });
    }

    public static Task SignOutRoomAsync(this HttpContext context)
    {
        return context.SignOutAsync(ApplicationConstants.RoomScheme);
    }


    public static Guid GetId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    public static int GetViewerId(this ClaimsPrincipal user)
    {
        return int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
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

    public static Guid GetRoomId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("RoomId")!);
    }

    public static RoomType GetRoomType(this ClaimsPrincipal user)
    {
        return Enum.Parse<RoomType>(user.FindFirstValue("RoomType")!);
    }
}