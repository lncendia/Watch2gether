using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Films.Entities;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Models.Country;
using Overoom.Infrastructure.Storage.Models.Film;
using Overoom.Infrastructure.Storage.Models.Genre;
using Overoom.Infrastructure.Storage.Models.Person;
using Overoom.Infrastructure.Storage.Models.Voice;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class FilmModelMapper : IModelMapperUnit<FilmModel, Film>
{
    private readonly ApplicationDbContext _context;

    public FilmModelMapper(ApplicationDbContext context) => _context = context;
    private readonly List<GenreModel> _genres = new();
    private readonly List<CountryModel> _countries = new();
    private readonly List<PersonModel> _persons = new();
    private readonly List<VoiceModel> _voices = new();

    public async Task<FilmModel> MapAsync(Film entity)
    {
        var film = _context.Films.Local.FirstOrDefault(x => x.Id == entity.Id);
        if (film == null)
        {
            film = await _context.Films.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (film != null)
            {
                await _context.Entry(film).Collection(x => x.Countries).LoadAsync();
                await _context.Entry(film).Collection(x => x.Directors).LoadAsync();
                await _context.Entry(film).Collection(x => x.Actors).Query().Include(x => x.Person).LoadAsync();
                await _context.Entry(film).Collection(x => x.Genres).LoadAsync();
                await _context.Entry(film).Collection(x => x.ScreenWriters).LoadAsync();
                await _context.Entry(film).Collection(x => x.CdnList).Query().Include(x => x.Voices).LoadAsync();
            }
            else
            {
                film = new FilmModel
                {
                    Id = entity.Id,
                    Type = entity.Type,
                    Name = entity.Name,
                    Year = entity.Year
                };
                await ProcessGenresAsync(entity, film);
                await ProcessActorsAsync(entity, film);
                await ProcessCountriesAsync(entity, film);
                await ProcessDirectorsAsync(entity, film);
                await ProcessScreenwritersAsync(entity, film);
            }
        }

        film.PosterUri = entity.PosterUri;
        film.Description = entity.Description;
        film.ShortDescription = entity.ShortDescription; //todo:get with reflection
        film.Rating = entity.Rating;
        film.UserRating = entity.UserRating;
        film.CountSeasons = entity.CountSeasons;
        film.CountEpisodes = entity.CountEpisodes;
        film.UserRatingsCount = entity.UserRatingsCount;

        await ProcessCdnAsync(entity, film);

        return film;
    }

    private async Task ProcessCdnAsync(Film entity, FilmModel film)
    {
        _context.FilmCdns.RemoveRange(film.CdnList.Where(x => entity.CdnList.All(cdn => cdn.Type != x.Type)));

        foreach (var cdn in entity.CdnList)
        {
            var cdnModel = film.CdnList.FirstOrDefault(x => x.Type == cdn.Type) ?? new CdnModel();

            cdnModel.Uri = cdn.Uri;
            cdnModel.Quality = cdn.Quality;
            cdnModel.Type = cdn.Type;

            var newVoices = cdn.Voices.Where(x => cdnModel.Voices.All(m => !string.Equals(m.Name, x, StringComparison.CurrentCultureIgnoreCase))).ToList();
            var removeVoices = cdnModel.Voices.Where(x => cdn.Voices.All(m => !string.Equals(m, x.Name, StringComparison.CurrentCultureIgnoreCase)));
            var databaseVoices = await _context.Voices.Where(x => newVoices.Select(s => s.ToUpper()).Any(v => v == x.Name.ToUpper()))
                .ToListAsync();
            databaseVoices =
                databaseVoices.Concat(_voices.Where(x => newVoices.Any(v => string.Equals(v, x.Name, StringComparison.CurrentCultureIgnoreCase)))).ToList();
            var notDatabaseVoices = newVoices.Where(x => databaseVoices.All(v => !string.Equals(v.Name, x, StringComparison.CurrentCultureIgnoreCase)))
                .Select(v => new VoiceModel { Name = v }).ToList();
            _voices.AddRange(notDatabaseVoices);
            cdnModel.Voices.AddRange(databaseVoices.Concat(notDatabaseVoices));
            cdnModel.Voices.RemoveAll(g => removeVoices.Contains(g));

            if (cdnModel.Id == default) film.CdnList.Add(cdnModel);
        }
    }

    private async Task ProcessCountriesAsync(Film entity, FilmModel film)
    {
        var newCountries =
            entity.FilmTags.Countries.Where(x => film.Countries.All(m => !string.Equals(m.Name, x, StringComparison.CurrentCultureIgnoreCase))).ToList();
        var databaseCountries = await _context.Countries
            .Where(x => newCountries.Select(s => s.ToUpper()).Any(c => c == x.Name.ToUpper()))
            .ToListAsync();
        databaseCountries =
            databaseCountries.Concat(_countries.Where(x => newCountries.Any(c => string.Equals(c, x.Name, StringComparison.CurrentCultureIgnoreCase))))
                .ToList();
        var notDatabaseCountries = newCountries.Where(x => databaseCountries.All(c => !string.Equals(c.Name, x, StringComparison.CurrentCultureIgnoreCase)))
            .Select(c => new CountryModel { Name = c }).ToList();
        _countries.AddRange(notDatabaseCountries);
        film.Countries.AddRange(databaseCountries.Concat(notDatabaseCountries));
    }

    private async Task ProcessActorsAsync(Film entity, FilmModel film)
    {
        var newActors =
            entity.FilmTags.Actors.Where(x => film.Actors.All(m => m.Person.Name.ToUpper() != x.ActorName.ToUpper()))
                .ToList();
        var databaseActors = await _context.Persons
            .Where(x => newActors.Select(s => s.ActorName.ToUpper()).Any(p => p == x.Name.ToUpper()))
            .ToListAsync();
        databaseActors =
            databaseActors.Concat(_persons.Where(x => newActors.Any(p => p.ActorName.ToUpper() == x.Name.ToUpper())))
                .ToList();
        var notDatabaseActors = newActors.Where(x => databaseActors.All(p => p.Name.ToUpper() != x.ActorName.ToUpper()))
            .Select(p => new PersonModel { Name = p.ActorName }).ToList();
        _persons.AddRange(notDatabaseActors);
        _context.Persons.AddRange(notDatabaseActors);
        var filmActors = databaseActors.Concat(notDatabaseActors)
            .Select(p => new FilmActorModel
                { Person = p, Description = newActors.First(a => a.ActorName == p.Name).ActorDescription });
        film.Actors.AddRange(filmActors);
    }

    private async Task ProcessGenresAsync(Film entity, FilmModel film)
    {
        var newGenres =
            entity.FilmTags.Genres.Where(x => film.Genres.All(m => !string.Equals(m.Name, x, StringComparison.CurrentCultureIgnoreCase))).ToList();
        var databaseGenres = await _context.Genres
            .Where(x => newGenres.Select(s => s.ToUpper()).Any(g => g == x.Name.ToUpper()))
            .ToListAsync();
        databaseGenres =
            databaseGenres.Concat(_genres.Where(x => newGenres.Any(g => string.Equals(g, x.Name, StringComparison.CurrentCultureIgnoreCase))))
                .ToList();
        var notDatabaseGenres = newGenres.Where(x => databaseGenres.All(g => !string.Equals(g.Name, x, StringComparison.CurrentCultureIgnoreCase)))
            .Select(g => new GenreModel { Name = g }).ToList();
        _genres.AddRange(notDatabaseGenres);
        film.Genres.AddRange(databaseGenres.Concat(notDatabaseGenres));
    }

    private async Task ProcessDirectorsAsync(Film entity, FilmModel film)
    {
        var newDirectors =
            entity.FilmTags.Directors.Where(x => film.Directors.All(m => !string.Equals(m.Name, x, StringComparison.CurrentCultureIgnoreCase))).ToList();
        var databaseDirectors = await _context.Persons
            .Where(x => newDirectors.Select(s => s.ToUpper()).Any(p => p == x.Name.ToUpper()))
            .ToListAsync();
        databaseDirectors =
            databaseDirectors.Concat(_persons.Where(x => newDirectors.Any(p => string.Equals(p, x.Name, StringComparison.CurrentCultureIgnoreCase))))
                .ToList();
        var notDatabaseDirectors = newDirectors.Where(x => databaseDirectors.All(p => !string.Equals(p.Name, x, StringComparison.CurrentCultureIgnoreCase)))
            .Select(p => new PersonModel { Name = p }).ToList();
        _persons.AddRange(notDatabaseDirectors);
        film.Directors.AddRange(databaseDirectors.Concat(notDatabaseDirectors));
    }

    private async Task ProcessScreenwritersAsync(Film entity, FilmModel film)
    {
        var newScreenwriters =
            entity.FilmTags.Screenwriters.Where(x => film.ScreenWriters.All(m => !string.Equals(m.Name, x, StringComparison.CurrentCultureIgnoreCase)))
                .ToList();
        var databaseScreenwriters = await _context.Persons
            .Where(x => newScreenwriters.Select(s => s.ToUpper()).Any(p => p == x.Name.ToUpper()))
            .ToListAsync();
        databaseScreenwriters =
            databaseScreenwriters
                .Concat(_persons.Where(x => newScreenwriters.Any(p => string.Equals(p, x.Name, StringComparison.CurrentCultureIgnoreCase))))
                .ToList();
        var notDatabaseScreenwriters = newScreenwriters
            .Where(x => databaseScreenwriters.All(p => !string.Equals(p.Name, x, StringComparison.CurrentCultureIgnoreCase)))
            .Select(p => new PersonModel { Name = p }).ToList();
        _persons.AddRange(notDatabaseScreenwriters);
        film.ScreenWriters.AddRange(databaseScreenwriters.Concat(notDatabaseScreenwriters));
    }
}