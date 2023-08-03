using Newtonsoft.Json;
using Overoom.Application.Abstractions.MovieApi.DTOs;
using Overoom.Application.Abstractions.MovieApi.Exceptions;
using Overoom.Infrastructure.Movie.Abstractions;
using Overoom.Infrastructure.Movie.Converters;
using Overoom.Infrastructure.Movie.Enums;
using Overoom.Infrastructure.Movie.Models;
using Episode = Overoom.Application.Abstractions.MovieApi.DTOs.Episode;
using Season = Overoom.Application.Abstractions.MovieApi.DTOs.Season;

namespace Overoom.Infrastructure.Movie.Services;

public class KpResponseParser : IKpResponseParser
{
    private readonly JsonSerializerSettings _settings = new()
        { Converters = { new ListObjectsConverter(), new ProfessionConverter() } };


    public IReadOnlyCollection<Season> GetSeasons(string json)
    {
        var seasons = JsonConvert.DeserializeObject<SeasonsResponse>(json, _settings)!;
        if (!seasons.Seasons.Any()) throw new ApiNotFoundException();
        return seasons.Seasons.Select(x =>
                new Season(x.Number, x.Episodes.Select(s => new Episode(s.EpisodeNumber, s.ReleaseDate)).ToList()))
            .ToList();
    }

    public FilmStaff GetStaff(string json)
    {
        var persons = JsonConvert.DeserializeObject<List<Person>>(json, _settings)!;
        var directors = persons.Where(x => x.Profession == Profession.Director)
            .Select(x => GetName(x.Name, x.NameEn)).ToList();
        var actors = persons.Where(x => x.Profession == Profession.Actor)
            .Select(x => (GetName(x.Name, x.NameEn), x.Description)).ToList();
        var screenwriters = persons.Where(x => x.Profession == Profession.Writer)
            .Select(x => GetName(x.Name, x.NameEn)).ToList();
        return new FilmStaff(directors, screenwriters, actors);
    }

    public Film GetFilm(string json)
    {
        var film = JsonConvert.DeserializeObject<FilmData>(json, _settings)!;
        var poster = string.IsNullOrEmpty(film.PosterUrl) ? null : new Uri(film.PosterUrl);
        return new Film(film.KpId, film.ImdbId, GetName(film.NameRu, film.NameEn), film.Year, film.Serial,
            film.Description, film.ShortDescription, poster, film.RatingKinopoisk, film.RatingImdb,
            film.Countries, film.Genres);
    }

    public IReadOnlyCollection<FilmShort> GetFilms(string json)
    {
        var films = JsonConvert.DeserializeObject<FilmSearchResponse>(json, _settings)!;
        if (!films.Items.Any()) throw new ApiNotFoundException();
        return films.Items.Select(film => new FilmShort(film.KpId, film.ImdbId, GetName(film.NameRu, film.NameEn)))
            .ToList();
    }


    private static string GetName(string? name, string? nameEn) =>
        (string.IsNullOrEmpty(name) ? nameEn : name) ?? string.Empty;
}