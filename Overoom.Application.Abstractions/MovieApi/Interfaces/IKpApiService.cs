using Overoom.Application.Abstractions.MovieApi.DTOs;

namespace Overoom.Application.Abstractions.MovieApi.Interfaces;

public interface IKpApiService
{
    Task<Film> GetAsync(long kpId, CancellationToken token = default);
    Task<FilmStaff> GetActorsAsync(long kpId, CancellationToken token = default);
    Task<IReadOnlyCollection<Season>> GetSeasonsAsync(long kpId, CancellationToken token = default);
    Task<IReadOnlyCollection<FilmShort>> FindAsync(string? title, string? imdbId, int page = 1, CancellationToken token = default);
}