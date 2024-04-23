using Microsoft.Extensions.DependencyInjection;
using PJMS.AuthService.Data.IdentityServer.DbContexts;
using PJMS.AuthService.Data.IdentityServer.Options;
using PJMS.AuthService.Data.IdentityServer.TokenCleanup;

namespace PJMS.AuthService.Data.IdentityServer.HostExtensions; 

/// <summary> 
/// Методы расширения для добавления поддержки базы данных EF в IdentityServer. 
/// </summary> 
public static class IdentityServerDbContextsExtensions
{ 
    /// <summary> 
    /// Добавляет Configuration DbContext в систему DI. 
    /// </summary> 
    /// <param name="services"></param> 
    /// <param name="storeOptionsAction">Действие для настройки параметров хранилища.</param> 
    /// <returns></returns> 
    public static IServiceCollection AddConfigurationDbContext(this IServiceCollection services, 
        Action<ConfigurationStoreOptions> storeOptionsAction = null) 
    { 
        var options = new ConfigurationStoreOptions(); 
        services.AddSingleton(options); 
        storeOptionsAction?.Invoke(options); 
        if (options.ResolveDbContextOptions != null) 
        { 
            services.AddDbContext<ConfigurationDbContext>(options.ResolveDbContextOptions); 
        } 
        else 
        { 
            services.AddDbContext<ConfigurationDbContext>(dbCtxBuilder => 
            { 
                options.ConfigureDbContext?.Invoke(dbCtxBuilder); 
            }); 
        } 
        return services; 
    } 
    /// <summary> 
    /// Добавляет operational DbContext в систему DI. 
    /// </summary> 
    /// <param name="services"></param> 
    /// <param name="storeOptionsAction">Действие для настройки параметров хранилища.</param> 
    /// <returns></returns> 
    public static IServiceCollection AddOperationalDbContext(this IServiceCollection services, 
        Action<OperationalStoreOptions> storeOptionsAction = null) 
    { 
        var storeOptions = new OperationalStoreOptions(); 
        services.AddSingleton(storeOptions); 
        storeOptionsAction?.Invoke(storeOptions); 
        if (storeOptions.ResolveDbContextOptions != null) 
        { 
            services.AddDbContext<PersistedGrantDbContext>(storeOptions.ResolveDbContextOptions); 
        } 
        else 
        { 
            services.AddDbContext<PersistedGrantDbContext>(dbCtxBuilder => 
            { 
                storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder); 
            }); 
        }

        if (!storeOptions.EnableTokenCleanup) return services;
        
        services.AddSingleton(storeOptions);
        services.AddTransient<TokenCleanupService>();
        services.AddHostedService<TokenCleanupHost>();

        return services; 
    } 
}