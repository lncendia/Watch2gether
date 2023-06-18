using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Rooms.DTOs;

namespace Overoom.WEB.RoomAuthentication;

public class RoomAuthentication
{
    public static async Task AuthenticateAsync(HttpContext context, ViewerDto viewer, Guid roomId, RoomType type)
    {
        await context.SignInAsync(ApplicationConstants.RoomScheme,
            new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, viewer.Username),
                new Claim(ClaimTypes.NameIdentifier, viewer.Id.ToString()),
                new Claim(ClaimTypes.Thumbprint, viewer.AvatarUrl),
                new Claim("RoomId", roomId.ToString()),
                new Claim("RoomType", type.ToString())
            }, ApplicationConstants.RoomScheme)));
    }
}