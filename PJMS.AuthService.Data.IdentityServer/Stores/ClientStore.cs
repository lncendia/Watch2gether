using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PJMS.AuthService.Data.IdentityServer.DbContexts;
using PJMS.AuthService.Data.IdentityServer.Mappers;

namespace PJMS.AuthService.Data.IdentityServer.Stores;

/// <summary>
/// Implementation of IClientStore thats uses EF.
/// </summary>
/// <seealso cref="IdentityServer4.Stores.IClientStore" />
public class ClientStore : IClientStore
{
    /// <summary>
    /// The DbContext.
    /// </summary>
    private readonly ConfigurationDbContext _context;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<ClientStore> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientStore"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">context</exception>
    public ClientStore(ConfigurationDbContext context, ILogger<ClientStore> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }

    /// <summary>
    /// Finds a client by id
    /// </summary>
    /// <param name="clientId">The client id</param>
    /// <returns>
    /// The client
    /// </returns>
    public async Task<Client> FindClientByIdAsync(string clientId)
    {
        var client = await _context.Clients
            .LoadDependencies()
            .Where(x => x.ClientId == clientId)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ClientId == clientId);

        if (client == null) return null;

        var model = client.ToModel();

        _logger.LogDebug("{ClientId} found in database: {ClientIdFound}", clientId, model != null);

        return model;
    }
}