using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Rooms.DTOs;

namespace Overoom.WEB.RoomAuthentication;

public static class Extensions
{
    public static async Task AuthenticateViewerAsync(this HttpContext context, ViewerDto viewer, Guid roomId,
        RoomType type)
    {
        await context.SignInAsync(ApplicationConstants.RoomScheme,
            new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, viewer.Username),
                new Claim(ClaimTypes.NameIdentifier, viewer.Id.ToString()),
                new Claim(ClaimTypes.Thumbprint, viewer.AvatarUrl.ToString()),
                new Claim("RoomId", roomId.ToString()),
                new Claim("RoomType", type.ToString())
            }, ApplicationConstants.RoomScheme)));
    }

    public static Guid GetUserId(this HttpContext context)
    {
        return Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
    
    public static bool IsAdmin(this HttpContext context)
    {
        return context.User.IsInRole(ApplicationConstants.AdminRoleName);
    }
}