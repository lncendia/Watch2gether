using Overoom.Application.Abstractions.Kinopoisk.DTOs;

namespace Overoom.Infrastructure.Movie.Abstractions;

public interface IResponseParser
{
    public IReadOnlyCollection<Season> GetSeasons(string json);
    public FilmStaff GetStaff(string json);
    public Film GetFilm(string json);

    public FilmShort GetFirstFilmFromSearch(string json);
}