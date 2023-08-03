using Overoom.Application.Abstractions.MovieApi.DTOs;

namespace Overoom.Application.Abstractions.MovieApi.Interfaces;

public interface IVideoCdnApiService
{
    Task<Cdn> GetInfoAsync(long kpId, CancellationToken token = default);
}