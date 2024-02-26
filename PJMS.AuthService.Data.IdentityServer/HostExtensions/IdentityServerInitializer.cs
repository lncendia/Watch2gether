using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PJMS.AuthService.Data.IdentityServer.DbContexts;
using PJMS.AuthService.Data.IdentityServer.Mappers;

namespace PJMS.AuthService.Data.IdentityServer.HostExtensions;

public static class IdentityServerInitializer
{
    /// <summary>
    /// Инициализация начальных данных в базу данных
    /// </summary>
    /// <param name="scopeServiceProvider">Определяет механизм для извлечения объекта службы,
    /// т. е. объекта, обеспечивающего настраиваемую поддержку для других объектов.</param>
    public static async Task InitAsync(IServiceProvider scopeServiceProvider)
    {
        using var serviceScope = scopeServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        await serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();

        var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        await context.Database.MigrateAsync();
        if (!context.Clients.Any())
        {
            foreach (var client in IdentityServerConfiguration.GetClients())
            {
                context.Clients.Add(client.ToEntity());
            }

            await context.SaveChangesAsync();
        }

        if (!context.IdentityResources.Any())
        {
            foreach (var resource in IdentityServerConfiguration.GetIdentityResources())
            {
                context.IdentityResources.Add(resource.ToEntity());
            }

            await context.SaveChangesAsync();
        }

        if (!context.ApiScopes.Any())
        {
            foreach (var resource in IdentityServerConfiguration.GetApiScopes())
            {
                context.ApiScopes.Add(resource.ToEntity());
            }

            await context.SaveChangesAsync();
        }

        if (!context.ApiResources.Any())
        {
            foreach (var resource in IdentityServerConfiguration.GetApiResources())
            {
                context.ApiResources.Add(resource.ToEntity());
            }

            await context.SaveChangesAsync();
        }
    }
}