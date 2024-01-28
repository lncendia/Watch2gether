using AuthService.Application.Abstractions.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Storage;

public class ApplicationContext(DbContextOptions<ApplicationContext> options)
    : IdentityDbContext<UserData, RoleData, long>(options)
{
    public DbSet<UserData>? ApplicationUsers { get; set; }
    public DbSet<RoleData>? ApplicationRoles { get; set; }
}