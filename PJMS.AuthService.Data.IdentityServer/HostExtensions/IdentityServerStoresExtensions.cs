using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using PJMS.AuthService.Data.IdentityServer.HostExtensions.Services;
using PJMS.AuthService.Data.IdentityServer.Options;
using PJMS.AuthService.Data.IdentityServer.Stores;

namespace PJMS.AuthService.Data.IdentityServer.HostExtensions;

/// <summary>
/// Extension methods to add EF database support to IdentityServer.
/// </summary>
public static class IdentityServerStoresExtensions
{
    /// <summary>
    /// Configures EF implementation of IClientStore, IResourceStore, and ICorsPolicyService with IdentityServer.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddConfigurationStore(this IIdentityServerBuilder builder,
        Action<ConfigurationStoreOptions> storeOptionsAction = null)
    {
        builder.Services.AddConfigurationDbContext(storeOptionsAction);

        builder.AddClientStore<ClientStore>();
        builder.AddResourceStore<ResourceStore>();
        builder.AddCorsPolicyService<CorsPolicyService>();

        return builder;
    }

    /// <summary>
    /// Configures EF implementation of IPersistedGrantStore with IdentityServer.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddOperationalStore(this IIdentityServerBuilder builder,
        Action<OperationalStoreOptions> storeOptionsAction = null)
    {
        builder.Services.AddOperationalDbContext(storeOptionsAction);

        builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
        builder.Services.AddTransient<IDeviceFlowStore, DeviceFlowStore>();

        return builder;
    }
}