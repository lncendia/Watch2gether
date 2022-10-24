using Microsoft.AspNetCore.Identity;

namespace Overoom.Application.Abstractions.Entities.User;

public sealed class UserData : IdentityUser
{
    public UserData(string email)
    {
        Email = email;
        UserName = email;
    }
}