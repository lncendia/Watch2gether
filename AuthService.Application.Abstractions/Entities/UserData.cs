using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Abstractions.Entities;

public sealed class UserData : IdentityUser<long>
{
    public UserData(string userName, string email, Uri avatarUrl, DateTime timeLastAuth) : base(userName)
    {
        AvatarUrl = avatarUrl;
        TimeLastAuth = timeLastAuth;
        Email = email;
    }

    public Uri AvatarUrl { get; set; }
    public DateTime TimeLastAuth { get; set; }
}