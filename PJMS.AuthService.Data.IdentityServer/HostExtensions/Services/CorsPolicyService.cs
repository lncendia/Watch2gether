using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PJMS.AuthService.Data.IdentityServer.DbContexts;

namespace PJMS.AuthService.Data.IdentityServer.HostExtensions.Services;

/// <summary>
/// Implementation of ICorsPolicyService that consults the client configuration in the database for allowed CORS origins.
/// </summary>
/// <seealso cref="IdentityServer4.Services.ICorsPolicyService" />
public class CorsPolicyService : ICorsPolicyService
{
    private readonly ConfigurationDbContext _context;
    private readonly ILogger<CorsPolicyService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CorsPolicyService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">context</exception>
    public CorsPolicyService(ConfigurationDbContext context, ILogger<CorsPolicyService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Determines whether origin is allowed.
    /// </summary>
    /// <param name="origin">The origin.</param>
    /// <returns></returns>
    public async Task<bool> IsOriginAllowedAsync(string origin)
    {
        origin = origin.ToLowerInvariant();

        var query = from o in _context.ClientCorsOrigins
            where o.Origin == origin
            select o;
            
        var isAllowed = await query.AnyAsync();

        _logger.LogDebug("Origin {Origin} is allowed: {OriginAllowed}", origin, isAllowed);

        return isAllowed;
    }
}