using Microsoft.AspNetCore.Identity;

namespace Overoom.Application.Abstractions.User.Entities.Role;

public class RoleData : IdentityRole
{
    public RoleData(string name) : base(name)
    {
    }
}