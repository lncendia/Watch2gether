using Films.Application.Abstractions.Services.MovieApi.DTOs;

namespace Films.Application.Abstractions.Services.MovieApi.Interfaces;

public interface IBazonApiService
{
    Task<CdnApiResponse> GetInfoAsync(long kpId, CancellationToken token = default);
}