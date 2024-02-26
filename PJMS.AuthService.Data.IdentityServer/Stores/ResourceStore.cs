using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PJMS.AuthService.Data.IdentityServer.DbContexts;
using PJMS.AuthService.Data.IdentityServer.Mappers;

namespace PJMS.AuthService.Data.IdentityServer.Stores;

/// <summary>
/// Implementation of IResourceStore thats uses EF.
/// </summary>
/// <seealso cref="IdentityServer4.Stores.IResourceStore" />
public class ResourceStore : IResourceStore
{
    /// <summary>
    /// The DbContext.
    /// </summary>
    private readonly ConfigurationDbContext _context;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<ResourceStore> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceStore"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">context</exception>
    public ResourceStore(ConfigurationDbContext context, ILogger<ResourceStore> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }

    /// <summary>
    /// Finds the API resources by name.
    /// </summary>
    /// <param name="apiResourceNames">The names.</param>
    /// <returns></returns>
    public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(
        IEnumerable<string> apiResourceNames)
    {
        ArgumentNullException.ThrowIfNull(apiResourceNames);

        var queryResult = await _context.ApiResources
            .Where(a => apiResourceNames.Contains(a.Name))
            .LoadDependencies()
            .AsNoTracking()
            .ToArrayAsync();

        var result = queryResult.Select(x => x.ToModel()).ToArray();

        if (result.Length != 0)
        {
            _logger.LogDebug("Found {Apis} API resource in database", result.Select(x => x.Name));
        }
        else
        {
            _logger.LogDebug("Did not find {Apis} API resource in database", apiResourceNames);
        }

        return result;
    }

    /// <summary>
    /// Gets API resources by scope name.
    /// </summary>
    /// <param name="scopeNames"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        var names = scopeNames.ToArray();

        var queryResult = await _context.ApiResources
            .Where(a => a.Scopes.Any(x => names.Contains(x.Scope)))
            .LoadDependencies()
            .AsNoTracking()
            .ToArrayAsync();

        var models = queryResult.Select(x => x.ToModel()).ToArray();

        _logger.LogDebug("Found {Apis} API resources in database", models.Select(x => x.Name));

        return models;
    }

    /// <summary>
    /// Gets identity resources by scope name.
    /// </summary>
    /// <param name="scopeNames"></param>
    /// <returns></returns>
    public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(
        IEnumerable<string> scopeNames)
    {
        var scopes = scopeNames.ToArray();

        var queryResult = await _context.IdentityResources
            .Where(r => scopes.Contains(r.Name))
            .LoadDependencies()
            .AsNoTracking()
            .ToArrayAsync();

        _logger.LogDebug("Found {Scopes} identity scopes in database", queryResult.Select(x => x.Name));

        return queryResult.Select(x => x.ToModel()).ToArray();
    }

    /// <summary>
    /// Gets scopes by scope name.
    /// </summary>
    /// <param name="scopeNames"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
    {
        var scopes = scopeNames.ToArray();

        var queryResult = await _context.ApiScopes
            .Where(s => scopes.Contains(s.Name))
            .LoadDependencies()
            .AsNoTracking()
            .ToArrayAsync();

        _logger.LogDebug("Found {Scopes} scopes in database", queryResult.Select(x => x.Name));

        return queryResult.Select(x => x.ToModel()).ToArray();
    }

    /// <summary>
    /// Gets all resources.
    /// </summary>
    /// <returns></returns>
    public async Task<Resources> GetAllResourcesAsync()
    {
        var identity = await _context.IdentityResources.LoadDependencies().AsNoTracking().ToArrayAsync();

        var apis = await _context.ApiResources.LoadDependencies().AsNoTracking().ToArrayAsync();

        var scopes = await _context.ApiScopes.LoadDependencies().AsNoTracking().ToArrayAsync();

        var result = new Resources(identity.Select(x => x.ToModel()), apis.Select(x => x.ToModel()),
            scopes.Select(x => x.ToModel()));

        _logger.LogDebug("Found {Scopes} as all scopes, and {Apis} as API resources",
            result.IdentityResources.Select(x => x.Name).Union(result.ApiScopes.Select(x => x.Name)),
            result.ApiResources.Select(x => x.Name));

        return result;
    }
}