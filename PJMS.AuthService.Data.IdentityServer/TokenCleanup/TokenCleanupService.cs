using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PJMS.AuthService.Data.IdentityServer.DbContexts;
using PJMS.AuthService.Data.IdentityServer.Options;

namespace PJMS.AuthService.Data.IdentityServer.TokenCleanup;

/// <summary>
/// Helper to cleanup stale persisted grants and device codes.
/// </summary>
public class TokenCleanupService
{
    private readonly OperationalStoreOptions _options;
    private readonly PersistedGrantDbContext _persistedGrantDbContext;
    private readonly ILogger<TokenCleanupService> _logger;

    /// <summary>
    /// Constructor for TokenCleanupService.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="persistedGrantDbContext"></param>
    /// <param name="logger"></param>
    public TokenCleanupService(OperationalStoreOptions options, PersistedGrantDbContext persistedGrantDbContext,
        ILogger<TokenCleanupService> logger)
    {
        _options = options;
        if (_options.TokenCleanupBatchSize < 1)
            throw new ArgumentException("Token cleanup batch size interval must be at least 1");
        _persistedGrantDbContext = persistedGrantDbContext;
        _logger = logger;
    }

    /// <summary>
    /// Method to clear expired persisted grants.
    /// </summary>
    /// <returns></returns>
    public async Task RemoveExpiredGrantsAsync()
    {
        try
        {
            await RemoveGrantsAsync();
            await RemoveDeviceCodesAsync();
            await SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception removing expired grants: {Exception}", ex.Message);
        }
    }

    /// <summary>
    /// Removes the stale persisted grants.
    /// </summary>
    /// <returns></returns>
    private async Task RemoveGrantsAsync()
    {
        var found = int.MaxValue;

        while (found >= _options.TokenCleanupBatchSize)
        {
            var expiredGrants = await _persistedGrantDbContext.PersistedGrants
                .Where(x => x.Expiration < DateTime.UtcNow)
                .OrderBy(x => x.Expiration)
                .Take(_options.TokenCleanupBatchSize)
                .ToArrayAsync();

            found = expiredGrants.Length;
            _logger.LogInformation("Removing {GrantCount} grants", found);

            _persistedGrantDbContext.PersistedGrants.RemoveRange(expiredGrants);
        }
    }


    /// <summary>
    /// Removes the stale device codes.
    /// </summary>
    /// <returns></returns>
    private async Task RemoveDeviceCodesAsync()
    {
        var found = int.MaxValue;

        while (found >= _options.TokenCleanupBatchSize)
        {
            var expiredCodes = await _persistedGrantDbContext.DeviceFlowCodes
                .Where(x => x.Expiration < DateTime.UtcNow)
                .OrderBy(x => x.Expiration)
                .Take(_options.TokenCleanupBatchSize)
                .ToArrayAsync();

            found = expiredCodes.Length;
            _logger.LogInformation("Removing {DeviceCodeCount} device flow codes", found);

            _persistedGrantDbContext.DeviceFlowCodes.RemoveRange(expiredCodes);
        }
    }

    private async Task SaveChangesAsync()
    {
        var count = 3;

        while (count > 0)
        {
            try
            {
                await _persistedGrantDbContext.SaveChangesAsync();
                return;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                count--;

                // we get this if/when someone else already deleted the records
                // we want to essentially ignore this, and keep working
                _logger.LogDebug("Concurrency exception removing expired grants: {Exception}", ex.Message);

                foreach (var entry in ex.Entries)
                {
                    // mark this entry as not attached anymore so we don't try to re-delete
                    entry.State = EntityState.Detached;
                }
            }
        }

        _logger.LogDebug("Too many concurrency exceptions. Exiting");
    }
}