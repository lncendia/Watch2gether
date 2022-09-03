using Microsoft.AspNetCore.Identity;

namespace Watch2gether.Application.Abstractions.Entities.Role;

public class RoleData : IdentityRole
{
    public RoleData(string name) : base(name)
    {
    }
}