using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Watch2gether.Application.Abstractions.Entities.Role;
using Watch2gether.Application.Abstractions.Entities.User;

namespace Watch2gether.Infrastructure.ApplicationData;

public class ApplicationContext : IdentityDbContext<UserData, RoleData, string>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<UserData>? ApplicationUsers { get; set; } = null!;
    public DbSet<RoleData>? ApplicationRoles { get; set; } = null!;
}