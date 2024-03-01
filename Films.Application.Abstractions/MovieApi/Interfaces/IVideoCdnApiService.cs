using Films.Application.Abstractions.MovieApi.DTOs;

namespace Films.Application.Abstractions.MovieApi.Interfaces;

public interface IVideoCdnApiService
{
    Task<CdnApiResponse> GetInfoAsync(long kpId, CancellationToken token = default);
}