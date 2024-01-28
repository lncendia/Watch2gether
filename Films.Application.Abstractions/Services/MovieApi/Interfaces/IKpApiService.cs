using Films.Application.Abstractions.Services.MovieApi.DTOs;

namespace Films.Application.Abstractions.Services.MovieApi.Interfaces;

public interface IKpApiService
{
    Task<FilmApiResponse> GetAsync(long kpId, CancellationToken token = default);
    Task<FilmStaffApiResponse> GetActorsAsync(long kpId, CancellationToken token = default);
    Task<IReadOnlyCollection<SeasonApiResponse>> GetSeasonsAsync(long kpId, CancellationToken token = default);
    Task<IReadOnlyCollection<FilmShortApiResponse>> FindByTitleAsync(string title, int page = 1, CancellationToken token = default);
    Task<IReadOnlyCollection<FilmShortApiResponse>> FindByImdbAsync(string imdbId, int page = 1, CancellationToken token = default);
}