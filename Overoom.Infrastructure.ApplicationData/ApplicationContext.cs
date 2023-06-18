using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Overoom.Application.Abstractions.Users.Entities;

namespace Overoom.Infrastructure.ApplicationData;

public class ApplicationContext : IdentityDbContext<UserData, RoleData, string>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<UserData>? ApplicationUsers { get; set; }
    public DbSet<RoleData>? ApplicationRoles { get; set; }
}