using Films.Infrastructure.Load.Kinopoisk.Models;

namespace Films.Infrastructure.Load.Kinopoisk;

public interface IKinopoiskApi
{
    Task<FilmData> GetAsync(long kpId, CancellationToken token = default);
    Task<IReadOnlyCollection<Person>> GetActorsAsync(long kpId, CancellationToken token = default);
    Task<SeasonsResponse> GetSeasonsAsync(long kpId, CancellationToken token = default);
    Task<FilmSearchResponse> FindByTitleAsync(string title, int? year, int page = 1, CancellationToken token = default);
    Task<FilmSearchResponse> FindByImdbAsync(string imdbId, CancellationToken token = default);
}