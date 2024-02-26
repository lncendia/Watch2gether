using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PJMS.AuthService.Data.IdentityServer.DbContexts;
using PJMS.AuthService.Data.IdentityServer.Mappers;

namespace PJMS.AuthService.Data.IdentityServer.Stores;

/// <summary>
/// Implementation of IPersistedGrantStore thats uses EF.
/// </summary>
/// <seealso cref="IdentityServer4.Stores.IPersistedGrantStore" />
public class PersistedGrantStore : IPersistedGrantStore
{
    /// <summary>
    /// The DbContext.
    /// </summary>
    private readonly PersistedGrantDbContext _context;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistedGrantStore"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="logger">The logger.</param>
    public PersistedGrantStore(PersistedGrantDbContext context, ILogger<PersistedGrantStore> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task StoreAsync(PersistedGrant token)
    {
        var existing = await _context.PersistedGrants.FirstOrDefaultAsync(x => x.Key == token.Key);
        if (existing == null)
        {
            _logger.LogDebug("{PersistedGrantKey} not found in database", token.Key);

            var persistedGrant = token.ToEntity();
            _context.PersistedGrants.Add(persistedGrant);
        }
        else
        {
            _logger.LogDebug("{PersistedGrantKey} found in database", token.Key);

            token.UpdateEntity(existing);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning("exception updating {PersistedGrantKey} persisted grant in database: {Error}", token.Key,
                ex.Message);
        }
    }

    /// <inheritdoc/>
    public async Task<PersistedGrant> GetAsync(string key)
    {
        var persistedGrant = await _context.PersistedGrants.AsNoTracking().FirstOrDefaultAsync(x => x.Key == key);
        var model = persistedGrant?.ToModel();

        _logger.LogDebug("{PersistedGrantKey} found in database: {PersistedGrantKeyFound}", key, model != null);

        return model;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
    {
        filter.Validate();

        var persistedGrants = await Filter(_context.PersistedGrants.AsQueryable(), filter)
            .AsNoTracking()
            .ToArrayAsync();

        var model = persistedGrants.Select(x => x.ToModel());

        _logger.LogDebug("{PersistedGrantCount} persisted grants found for {@Filter}", persistedGrants.Length, filter);

        return model;
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(string key)
    {
        var persistedGrant = await _context.PersistedGrants.FirstOrDefaultAsync(x => x.Key == key);
        if (persistedGrant != null)
        {
            _logger.LogDebug("removing {PersistedGrantKey} persisted grant from database", key);

            _context.PersistedGrants.Remove(persistedGrant);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogInformation("exception removing {PersistedGrantKey} persisted grant from database: {Error}",
                    key, ex.Message);
            }
        }
        else
        {
            _logger.LogDebug("no {PersistedGrantKey} persisted grant found in database", key);
        }
    }

    /// <inheritdoc/>
    public async Task RemoveAllAsync(PersistedGrantFilter filter)
    {
        filter.Validate();

        var persistedGrants = await Filter(_context.PersistedGrants.AsQueryable(), filter).ToArrayAsync();

        _logger.LogDebug("removing {PersistedGrantCount} persisted grants from database for {@Filter}",
            persistedGrants.Length, filter);

        _context.PersistedGrants.RemoveRange(persistedGrants);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogInformation(
                "removing {PersistedGrantCount} persisted grants from database for subject {@Filter}: {Error}",
                persistedGrants.Length, filter, ex.Message);
        }
    }


    private static IQueryable<Entities.PersistedGrant> Filter(IQueryable<Entities.PersistedGrant> query,
        PersistedGrantFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.ClientId))
        {
            query = query.Where(x => x.ClientId == filter.ClientId);
        }

        if (!string.IsNullOrWhiteSpace(filter.SessionId))
        {
            query = query.Where(x => x.SessionId == filter.SessionId);
        }

        if (!string.IsNullOrWhiteSpace(filter.SubjectId))
        {
            query = query.Where(x => x.SubjectId == filter.SubjectId);
        }

        if (!string.IsNullOrWhiteSpace(filter.Type))
        {
            query = query.Where(x => x.Type == filter.Type);
        }

        return query;
    }
}