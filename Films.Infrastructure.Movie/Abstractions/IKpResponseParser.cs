using Films.Application.Abstractions.Services.MovieApi.DTOs;

namespace Films.Infrastructure.Movie.Abstractions;

public interface IKpResponseParser
{
    public IReadOnlyCollection<SeasonApiResponse> GetSeasons(string json);
    public FilmStaffApiResponse GetStaff(string json);
    public FilmApiResponse GetFilm(string json);
    public IReadOnlyCollection<FilmShortApiResponse> GetFilms(string json);
}