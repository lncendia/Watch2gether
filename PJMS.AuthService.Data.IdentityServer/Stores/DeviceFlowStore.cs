using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Stores.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PJMS.AuthService.Data.IdentityServer.DbContexts;
using PJMS.AuthService.Data.IdentityServer.Entities;

namespace PJMS.AuthService.Data.IdentityServer.Stores;

/// <summary>
/// Implementation of IDeviceFlowStore thats uses EF.
/// </summary>
/// <seealso cref="IdentityServer4.Stores.IDeviceFlowStore" />
public class DeviceFlowStore : IDeviceFlowStore
{
    /// <summary>
    /// The DbContext.
    /// </summary>
    private readonly PersistedGrantDbContext _context;

    /// <summary>
    ///  The serializer.
    /// </summary>
    private readonly IPersistentGrantSerializer _serializer;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceFlowStore"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="serializer">The serializer</param>
    /// <param name="logger">The logger.</param>
    public DeviceFlowStore(
        PersistedGrantDbContext context,
        IPersistentGrantSerializer serializer,
        ILogger<DeviceFlowStore> logger)
    {
        _context = context;
        _serializer = serializer;
        _logger = logger;
    }

    /// <summary>
    /// Stores the device authorization request.
    /// </summary>
    /// <param name="deviceCode">The device code.</param>
    /// <param name="userCode">The user code.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public async Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data)
    {
        _context.DeviceFlowCodes.Add(ToEntity(data, deviceCode, userCode));

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Finds device authorization by user code.
    /// </summary>
    /// <param name="userCode">The user code.</param>
    /// <returns></returns>
    public async Task<DeviceCode> FindByUserCodeAsync(string userCode)
    {
        var deviceFlowCodes = await _context.DeviceFlowCodes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserCode == userCode);

        var model = ToModel(deviceFlowCodes?.Data);

        _logger.LogDebug("{UserCode} found in database: {UserCodeFound}", userCode, model != null);

        return model;
    }

    /// <summary>
    /// Finds device authorization by device code.
    /// </summary>
    /// <param name="deviceCode">The device code.</param>
    /// <returns></returns>
    public async Task<DeviceCode> FindByDeviceCodeAsync(string deviceCode)
    {
        var deviceFlowCodes = await _context.DeviceFlowCodes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DeviceCode == deviceCode);

        var model = ToModel(deviceFlowCodes?.Data);

        _logger.LogDebug("{DeviceCode} found in database: {DeviceCodeFound}", deviceCode, model != null);

        return model;
    }

    /// <summary>
    /// Updates device authorization, searching by user code.
    /// </summary>
    /// <param name="userCode">The user code.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public async Task UpdateByUserCodeAsync(string userCode, DeviceCode data)
    {
        var existing = await _context.DeviceFlowCodes.FirstOrDefaultAsync(x => x.UserCode == userCode);
        
        if (existing == null)
        {
            _logger.LogError("{UserCode} not found in database", userCode);
            throw new InvalidOperationException("Could not update device code");
        }

        var entity = ToEntity(data, existing.DeviceCode, userCode);
        _logger.LogDebug("{UserCode} found in database", userCode);

        existing.SubjectId = data.Subject?.FindFirst(JwtClaimTypes.Subject)?.Value;
        existing.Data = entity.Data;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning("exception updating {UserCode} user code in database: {Error}", userCode, ex.Message);
        }
    }

    /// <summary>
    /// Removes the device authorization, searching by device code.
    /// </summary>
    /// <param name="deviceCode">The device code.</param>
    /// <returns></returns>
    public async Task RemoveByDeviceCodeAsync(string deviceCode)
    {
        var deviceFlowCodes = await _context.DeviceFlowCodes.FirstOrDefaultAsync(x => x.DeviceCode == deviceCode);

        if (deviceFlowCodes != null)
        {
            _logger.LogDebug("removing {DeviceCode} device code from database", deviceCode);

            _context.DeviceFlowCodes.Remove(deviceFlowCodes);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogInformation("exception removing {DeviceCode} device code from database: {Error}", deviceCode,
                    ex.Message);
            }
        }
        else
        {
            _logger.LogDebug("no {DeviceCode} device code found in database", deviceCode);
        }
    }

    /// <summary>
    /// Converts a model to an entity.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="deviceCode"></param>
    /// <param name="userCode"></param>
    /// <returns></returns>
    private DeviceFlowCodes ToEntity(DeviceCode model, string deviceCode, string userCode)
    {
        return new DeviceFlowCodes
        {
            DeviceCode = deviceCode,
            UserCode = userCode,
            ClientId = model.ClientId,
            SubjectId = model.Subject?.FindFirst(JwtClaimTypes.Subject)?.Value,
            CreationTime = model.CreationTime,
            Expiration = model.CreationTime.AddSeconds(model.Lifetime),
            Data = _serializer.Serialize(model)
        };
    }

    /// <summary>
    /// Converts a serialized DeviceCode to a model.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    private DeviceCode ToModel(string entity)
    {
        return _serializer.Deserialize<DeviceCode>(entity);
    }
}