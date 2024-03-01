using Films.Application.Abstractions.MovieApi.DTOs;
using Films.Application.Abstractions.MovieApi.Exceptions;
using Newtonsoft.Json;
using Films.Infrastructure.Movie.Abstractions;
using Films.Infrastructure.Movie.Converters;
using Films.Infrastructure.Movie.Enums;
using Films.Infrastructure.Movie.Models;

namespace Films.Infrastructure.Movie.Services;

public class KpResponseParser : IKpResponseParser
{
    private readonly JsonSerializerSettings _settings = new()
        { Converters = { new ListObjectsConverter(), new ProfessionConverter() } };


    public IReadOnlyCollection<SeasonApiResponse> GetSeasons(string json)
    {
        var seasons = JsonConvert.DeserializeObject<SeasonsResponse>(json, _settings)!;
        if (seasons.Seasons.Count == 0) throw new ApiNotFoundException();
        return seasons.Seasons.Select(x =>
                new SeasonApiResponse
                {
                    Number = x.Number,
                    Episodes = x.Episodes.Select(s => new EpisodeApiResponse
                    {
                        EpisodeNumber = s.EpisodeNumber,
                        ReleaseDate = s.ReleaseDate
                    }).ToArray()
                })
            .ToArray();
    }

    public FilmStaffApiResponse GetStaff(string json)
    {
        var persons = JsonConvert.DeserializeObject<List<Person>>(json, _settings)!;
        var directors = persons.Where(x => x.Profession == Profession.Director)
            .Select(x => GetName(x.Name, x.NameEn)).ToArray();
        var actors = persons.Where(x => x.Profession == Profession.Actor)
            .Select(x => new ActorApiResponse
            {
                Name = GetName(x.Name, x.NameEn),
                Description = x.Description
            }).ToArray();
        var screenwriters = persons.Where(x => x.Profession == Profession.Writer)
            .Select(x => GetName(x.Name, x.NameEn)).ToArray();
        return new FilmStaffApiResponse
        {
            Directors = directors,
            ScreenWriters = screenwriters,
            Actors = actors
        };
    }

    public FilmApiResponse GetFilm(string json)
    {
        var film = JsonConvert.DeserializeObject<FilmData>(json, _settings)!;
        var poster = string.IsNullOrEmpty(film.PosterUrl) ? null : new Uri(film.PosterUrl);
        return new FilmApiResponse
        {
            KpId = film.KpId,
            ImdbId = film.ImdbId,
            Title = GetName(film.NameRu, film.NameEn),
            Year = film.Year,
            Serial = film.Serial,
            Countries = film.Countries,
            Genres = film.Genres,
            Description = film.Description,
            ShortDescription = film.ShortDescription,
            PosterUrl = poster,
            RatingKp = film.RatingKinopoisk,
            RatingImdb = film.RatingImdb
        };
    }

    public IReadOnlyCollection<FilmShortApiResponse> GetFilms(string json)
    {
        var films = JsonConvert.DeserializeObject<FilmSearchResponse>(json, _settings)!;
        if (films.Items.Count == 0) throw new ApiNotFoundException();
        return films.Items.Select(film => new FilmShortApiResponse
            {
                KpId = film.KpId,
                ImdbId = film.ImdbId,
                Title = GetName(film.NameRu, film.NameEn)
            })
            .ToArray();
    }


    private static string GetName(string? name, string? nameEn) =>
        (string.IsNullOrEmpty(name) ? nameEn : name) ?? string.Empty;
}