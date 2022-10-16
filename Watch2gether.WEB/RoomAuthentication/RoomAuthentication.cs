using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Watch2gether.Application.Abstractions;
using Watch2gether.Application.Abstractions.DTO.Rooms;

namespace Watch2gether.WEB.RoomAuthentication;

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