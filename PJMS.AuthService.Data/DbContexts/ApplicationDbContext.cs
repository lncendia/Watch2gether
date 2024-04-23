using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PJMS.AuthService.Abstractions.Entities;

namespace PJMS.AuthService.Data.DbContexts;

/// <summary>
/// Контекст базы данных
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<AppUser, AppRole, Guid>(options)
{
    /// <summary>
    /// Настраивает схему, необходимую для структуры идентификации.
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Вызываем базовую реализацию метода OnModelCreating
        base.OnModelCreating(builder);

        // Устанавливаем таблицу "Users" для сущности AppUser
        builder.Entity<AppUser>(entity => entity.ToTable(name: "Users"));

        // Устанавливаем таблицу "Roles" для сущности AppRole
        builder.Entity<AppRole>(entity => entity.ToTable(name: "Roles"));

        // Устанавливаем таблицу "UserRoles" для сущности IdentityUserRole<Guid>
        builder.Entity<IdentityUserRole<Guid>>(entity => entity.ToTable(name: "UserRoles"));

        // Устанавливаем таблицу "UserClaims" для сущности IdentityUserClaim<Guid>
        builder.Entity<IdentityUserClaim<Guid>>(entity => entity.ToTable(name: "UserClaims"));

        // Устанавливаем таблицу "UserLogins" для сущности IdentityUserLogin<Guid>
        builder.Entity<IdentityUserLogin<Guid>>(entity => entity.ToTable("UserLogins"));

        // Устанавливаем таблицу "UserTokens" для сущности IdentityUserToken<Guid>
        builder.Entity<IdentityUserToken<Guid>>(entity => entity.ToTable("UserTokens"));

        // Устанавливаем таблицу "RoleClaims" для сущности IdentityRoleClaim<Guid>
        builder.Entity<IdentityRoleClaim<Guid>>(entity => entity.ToTable("RoleClaims"));
    }
}