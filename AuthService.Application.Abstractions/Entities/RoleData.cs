using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Abstractions.Entities;

public class RoleData(string name) : IdentityRole<long>(name);