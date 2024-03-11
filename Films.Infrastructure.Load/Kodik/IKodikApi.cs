using Films.Infrastructure.Load.Kodik.Models;

namespace Films.Infrastructure.Load.Kodik;

public interface IKodikApi
{
    Task<IReadOnlyCollection<FilmData>> GetByTitleAsync(string title, bool includeMaterials, int? year,
        CancellationToken token = default);

    Task<IReadOnlyCollection<FilmData>> GetByKinopoiskIdAsync(long kpId, bool includeMaterials,
        CancellationToken token = default);

    Task<IReadOnlyCollection<FilmData>> GetByImdbIdAsync(string imdbId, bool includeMaterials,
        CancellationToken token = default);

    Task<IReadOnlyCollection<FilmData>> GetByMdlIdAsync(long mdlId, bool includeMaterials,
        CancellationToken token = default);

    Task<IReadOnlyCollection<FilmData>> GetByWorldArtAnimationIdAsync(long worldArtAnimationId, bool includeMaterials,
        CancellationToken token = default);

    Task<IReadOnlyCollection<FilmData>> GetByWorldArtCinemaIdAsync(long worldArtCinemaId, bool includeMaterials,
        CancellationToken token = default);

    Task<IReadOnlyCollection<FilmData>> GetByWorldArtLinkAsync(string worldArtLink, bool includeMaterials,
        CancellationToken token = default);

    Task<IReadOnlyCollection<FilmData>> GetByShikimoriIdAsync(long shikimoriId, bool includeMaterials,
        CancellationToken token = default);
}