using Overoom.Application.Abstractions.MovieApi.DTOs;

namespace Overoom.Infrastructure.Movie.Abstractions;

public interface IKpResponseParser
{
    public IReadOnlyCollection<Season> GetSeasons(string json);
    public FilmStaff GetStaff(string json);
    public Film GetFilm(string json);
    public IReadOnlyCollection<FilmShort> GetFilms(string json);
}