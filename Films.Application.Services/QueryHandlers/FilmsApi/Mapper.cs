using Films.Application.Abstractions.Queries.FilmsApi.DTOs;
using Films.Application.Abstractions.Services.MovieApi.DTOs;
using Films.Domain.Films.Enums;
using Films.Domain.Films.ValueObjects;

namespace Films.Application.Services.QueryHandlers.FilmsApi;

internal static class Mapper
{
    internal static FilmApiDto Map(FilmApiResponse filmApiResponse, FilmStaffApiResponse staffApiResponse,
        CdnApiResponse videoCdn, IReadOnlyCollection<SeasonApiResponse>? seasons)
    {
        return new FilmApiDto
        {
            Description = filmApiResponse.Description,
            ShortDescription = filmApiResponse.ShortDescription,
            Type = filmApiResponse.Serial ? FilmType.Serial : FilmType.Film,
            Title = filmApiResponse.Title,
            Year = filmApiResponse.Year,
            CountSeasons = seasons?.Count,
            CountEpisodes = seasons?.SelectMany(s => s.Episodes).Count(),
            Cdn = [Map(videoCdn, "VideoCdn")],
            Countries = filmApiResponse.Countries,
            Actors = staffApiResponse.Actors.Select(a => new Actor(a.Name, a.Description)).ToArray(),
            Directors = staffApiResponse.Directors,
            Genres = filmApiResponse.Genres,
            Screenwriters = staffApiResponse.ScreenWriters,
            RatingKp = filmApiResponse.RatingKp,
            RatingImdb = filmApiResponse.RatingImdb,
            PosterUrl = filmApiResponse.PosterUrl
        };
    }

    private static Cdn Map(CdnApiResponse cdn, string name) => new()
    {
        Name = name,
        Url = cdn.Url,
        Quality = cdn.Quality,
        Voices = cdn.Voices
    };
}