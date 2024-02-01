using Films.Application.Abstractions.Queries.FilmsApi.DTOs;
using Films.Application.Abstractions.Services.MovieApi.DTOs;
using Films.Domain.Films.Enums;
using Films.Domain.Films.ValueObjects;

namespace Films.Application.Services.Queries.FilmsApi;

internal static class Mapper
{
    internal static FilmApiDto Map(FilmApiResponse filmApiResponse, FilmStaffApiResponse staffApiResponse,
        CdnApiResponse videoCdn, CdnApiResponse bazonCdn, IReadOnlyCollection<SeasonApiResponse>? seasons)
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
            Cdn = [Map(videoCdn, CdnType.VideoCdn), Map(bazonCdn, CdnType.Bazon)],
            Countries = filmApiResponse.Countries,
            Actors = staffApiResponse.Actors.Select(a => new Actor
            {
                Name = a.Name,
                Description = a.Description
            }).ToArray(),
            Directors = staffApiResponse.Directors,
            Genres = filmApiResponse.Genres,
            Screenwriters = staffApiResponse.ScreenWriters,
            RatingKp = filmApiResponse.RatingKp,
            RatingImdb = filmApiResponse.RatingImdb
        };
    }

    private static Cdn Map(CdnApiResponse cdn, CdnType type) => new()
    {
        Type = type,
        Url = cdn.Url,
        Quality = cdn.Quality,
        Voices = cdn.Voices
    };
}