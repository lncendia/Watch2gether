using Microsoft.EntityFrameworkCore;
using Films.Domain.Films.Entities;
using Films.Domain.Films.ValueObjects;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Country;
using Films.Infrastructure.Storage.Models.Film;
using Films.Infrastructure.Storage.Models.Genre;
using Films.Infrastructure.Storage.Models.Person;
using Films.Infrastructure.Storage.Models.Voice;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

// ReSharper disable EntityFramework.NPlusOne.IncompleteDataUsage
// ReSharper disable EntityFramework.NPlusOne.IncompleteDataQuery
internal class FilmModelMapper(ApplicationDbContext context) : IModelMapperUnit<FilmModel, Film>
{
    public async Task<FilmModel> MapAsync(Film entity)
    {
        var film = await context.Films.FirstOrDefaultAsync(x => x.Id == entity.Id);
        if (film != null)
        {
            await context.Entry(film).Collection(x => x.Countries).LoadAsync();
            await context.Entry(film).Collection(x => x.Directors).LoadAsync();
            await context.Entry(film).Collection(x => x.Actors).Query().Include(x => x.Person).LoadAsync();
            await context.Entry(film).Collection(x => x.Genres).LoadAsync();
            await context.Entry(film).Collection(x => x.Screenwriters).LoadAsync();
            await context.Entry(film).Collection(x => x.CdnList).Query().Include(x => x.Voices).LoadAsync();
        }
        else
        {
            film = new FilmModel { Id = entity.Id };
        }

        film.Type = entity.Type;
        film.Title = entity.Title;
        film.Year = entity.Year;
        film.PosterUrl = entity.PosterUrl;
        film.Description = entity.Description;
        film.ShortDescription = entity.ShortDescription;
        film.RatingKp = entity.RatingKp;
        film.RatingImdb = entity.RatingImdb;
        film.UserRating = entity.UserRating;
        film.CountSeasons = entity.CountSeasons;
        film.CountEpisodes = entity.CountEpisodes;
        film.UserRatingsCount = entity.UserRatingsCount;

        var persons = await GetPersonsAsync(entity);

        ProcessActors(entity, film, persons);
        ProcessDirectors(entity, film, persons);
        ProcessScreenwriters(entity, film, persons);
        
        await ProcessGenresAsync(entity, film);
        await ProcessCountriesAsync(entity, film);
        await ProcessCdnsAsync(entity, film);

        return film;
    }

    private async Task<IReadOnlyCollection<PersonModel>> GetPersonsAsync(Film entity)
    {
        var persons = entity.Directors
            .Concat(entity.Screenwriters)
            .Concat(entity.Actors.Select(a => a.Name))
            .Distinct()
            .ToArray();

        var databasePersons = await context.Persons
            .Where(p => persons.Any(person => person == p.Name))
            .ToArrayAsync();

        // Определяем, какие модели не были получены из EF, для этих новых записей создаем новые модели EF
        var notDatabasePersons = persons
            .Where(c => databasePersons.All(d => d.Name != c))
            .Select(c => new PersonModel { Name = c })
            .ToArray();

        return databasePersons.Concat(notDatabasePersons).ToArray();
    }

    private async Task ProcessCdnsAsync(Film entity, FilmModel film)
    {
        // Получаем записи, которые есть в модели EF, но уже нет в сущности
        var removeCdns = film.CdnList
            .Where(x => entity.CdnList.All(m => x.Type != m.Type));

        // Удаляем эти записи из коллекции в модели EF
        film.CdnList.RemoveAll(g => removeCdns.Contains(g));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newCdns = entity.CdnList
            .Where(x => film.CdnList.All(m => x.Type != m.Type))
            .Select(c => new CdnModel { Type = c.Type })
            .ToArray();

        var voices = await GetVoicesAsync(entity);

        foreach (var cdnModel in film.CdnList)
        {
            ProcessCdn(entity.CdnList.First(c => c.Type == cdnModel.Type), cdnModel, voices);
        }

        foreach (var cdnModel in newCdns)
        {
            ProcessCdn(entity.CdnList.First(c => c.Type == cdnModel.Type), cdnModel, voices);
        }

        film.CdnList.AddRange(newCdns);
    }

    private async Task<IReadOnlyCollection<VoiceModel>> GetVoicesAsync(Film entity)
    {
        var voices = entity.CdnList.SelectMany(c => c.Voices).Distinct().ToArray();

        var databaseVoices = await context.Voices
            .Where(v => voices.Any(voice => voice == v.Name))
            .ToArrayAsync();

        // Определяем, какие модели не были получены из EF, для этих новых записей создаем новые модели EF
        var notDatabaseVoices = voices
            .Where(c => databaseVoices.All(d => d.Name != c))
            .Select(c => new VoiceModel { Name = c });

        return databaseVoices.Concat(notDatabaseVoices).ToArray();
    }

    private static void ProcessCdn(Cdn valueObject, CdnModel cdn, IEnumerable<VoiceModel> voices)
    {
        cdn.Quality = valueObject.Quality;
        cdn.Url = valueObject.Url;

        // Получаем записи, которые есть в модели EF, но уже нет в сущности
        var removeVoices = cdn.Voices
            .Where(x => valueObject.Voices.All(m => m != x.Name));

        // Удаляем эти записи из коллекции в модели EF
        cdn.Voices.RemoveAll(g => removeVoices.Contains(g));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newVoices = valueObject.Voices
            .Where(x => cdn.Voices.All(m => m.Name != x))
            .ToArray();

        // Если новых озвучек нет
        if (newVoices.Length == 0) return;

        // Запрашиваем все существующие модели EF для новых записей
        var databaseVoices = voices
            .Where(x => newVoices.Any(g => g == x.Name));

        // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
        cdn.Voices.AddRange(databaseVoices);
    }

    private async Task ProcessCountriesAsync(Film entity, FilmModel film)
    {
        // Получаем записи, которые есть в модели EF, но уже нет в сущности
        var removeCountries = film.Countries
            .Where(x => entity.Countries.All(m => m != x.Name));

        // Удаляем эти записи из коллекции в модели EF
        film.Countries.RemoveAll(g => removeCountries.Contains(g));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newCountries = entity.Countries
            .Where(x => film.Countries.All(m => m.Name != x))
            .ToArray();

        // Если новые записи есть
        if (newCountries.Length != 0)
        {
            // Запрашиваем все существующие модели EF для новых записей
            var databaseCountries = await context.Countries
                .Where(x => newCountries.Any(g => g == x.Name))
                .ToArrayAsync();

            // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
            film.Countries.AddRange(databaseCountries);

            // Определяем, какие модели не были получены из EF, для этих новых записей создаем новые модели EF
            var notDatabaseCountries = newCountries
                .Where(c => databaseCountries.All(d => d.Name != c))
                .Select(c => new CountryModel { Name = c });

            // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
            film.Countries.AddRange(notDatabaseCountries);
        }
    }

    private static void ProcessActors(Film entity, FilmModel film, IEnumerable<PersonModel> persons)
    {
        // Получаем записи, которые есть в модели EF, но уже нет в сущности
        var removeActors = film.Actors
            .Where(x => entity.Actors.All(m => m.Name != x.Person.Name));

        // Удаляем эти записи из коллекции в модели EF
        film.Actors.RemoveAll(g => removeActors.Contains(g));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newActors = entity.Actors
            .Where(x => film.Actors.All(m => m.Person.Name != x.Name))
            .ToArray();

        // Если новых записей нет
        if (newActors.Length == 0) return;

        // Получаем модели EF для новых записей
        var newPersons = persons
            .Where(x => newActors.Select(a => a.Name).Any(g => g == x.Name))
            .Select(p => new FilmActorModel
                { Person = p, Description = newActors.First(a => a.Name == p.Name).Description });

        // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
        film.Actors.AddRange(newPersons);
    }

    private async Task ProcessGenresAsync(Film entity, FilmModel film)
    {
        // Получаем записи, которые есть в модели EF, но уже нет в сущности
        var removeGenres = film.Genres
            .Where(x => entity.Genres.All(m => m != x.Name));

        // Удаляем эти записи из коллекции в модели EF
        film.Genres.RemoveAll(g => removeGenres.Contains(g));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newGenres = entity.Genres
            .Where(x => film.Genres.All(m => m.Name != x))
            .ToArray();

        // Если новые записи есть
        if (newGenres.Length != 0)
        {
            // Запрашиваем все существующие модели EF для новых записей
            var databaseGenres = await context.Genres
                .Where(x => newGenres.Any(g => g == x.Name))
                .ToArrayAsync();

            // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
            film.Genres.AddRange(databaseGenres);

            // Определяем, какие модели не были получены из EF, для этих новых записей создаем новые модели EF
            var notDatabaseGenres = newGenres
                .Where(c => databaseGenres.All(d => d.Name != c))
                .Select(c => new GenreModel { Name = c });

            // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
            film.Genres.AddRange(notDatabaseGenres);
        }
    }

    private static void ProcessDirectors(Film entity, FilmModel film, IEnumerable<PersonModel> persons)
    {
        // Получаем записи, которые есть в модели EF, но уже нет в сущности
        var removeDirectors = film.Directors
            .Where(x => entity.Directors.All(m => m != x.Name));

        // Удаляем эти записи из коллекции в модели EF
        film.Directors.RemoveAll(g => removeDirectors.Contains(g));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newDirectors = entity.Directors
            .Where(x => film.Directors.All(m => m.Name != x))
            .ToArray();

        // Если новых записей нет
        if (newDirectors.Length == 0) return;

        // Получаем модели EF для новых записей
        var newPersons = persons.Where(x => newDirectors.Any(g => g == x.Name));

        // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
        film.Directors.AddRange(newPersons);
    }

    private static void ProcessScreenwriters(Film entity, FilmModel film, IEnumerable<PersonModel> persons)
    {
        // Получаем записи, которые есть в модели EF, но уже нет в сущности
        var removeScreenwriters = film.Screenwriters
            .Where(x => entity.Screenwriters.All(m => m != x.Name));

        // Удаляем эти записи из коллекции в модели EF
        film.Screenwriters.RemoveAll(g => removeScreenwriters.Contains(g));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newScreenwriters = entity.Screenwriters
            .Where(x => film.Screenwriters.All(m => m.Name != x))
            .ToArray();

        // Если новых записей нет
        if (newScreenwriters.Length == 0) return;

        // Получаем модели EF для новых записей
        var newPersons = persons.Where(x => newScreenwriters.Any(g => x.Name == g));

        // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
        film.Screenwriters.AddRange(newPersons);
    }
}