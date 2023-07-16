using Overoom.Application.Abstractions.Kinopoisk.DTOs;

namespace Overoom.Application.Abstractions.Kinopoisk.Interfaces;

public interface IKpApiService
{
    Task<FilmShort> GetFromTitleAsync(string title);
    Task<FilmShort> GetFromImdbAsync(string imdbId);
    Task<DTOs.Film> GetFromKpAsync(long kpId);
    Task<FilmStaff> GetActorsAsync(long kpId);
    Task<IReadOnlyCollection<Season>> GetSeasonsAsync(long kpId);
}