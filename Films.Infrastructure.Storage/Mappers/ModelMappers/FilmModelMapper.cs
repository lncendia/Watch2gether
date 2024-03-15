using System.Reflection;
using Films.Domain.Films;
using Microsoft.EntityFrameworkCore;
using Films.Domain.Films.ValueObjects;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Extensions;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Countries;
using Films.Infrastructure.Storage.Models.Films;
using Films.Infrastructure.Storage.Models.Genres;
using Films.Infrastructure.Storage.Models.Persons;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

internal class FilmModelMapper(ApplicationDbContext context) : IModelMapperUnit<FilmModel, Film>
{
    private static readonly FieldInfo ShortDescription =
        typeof(Film).GetField("_shortDescription", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public async Task<FilmModel> MapAsync(Film aggregate)
    {
        var model = await context.Films
            .LoadDependencies()
            .FirstOrDefaultAsync(x => x.Id == aggregate.Id) ?? new FilmModel { Id = aggregate.Id };

        model.IsSerial = aggregate.IsSerial;
        model.Title = aggregate.Title;
        model.Year = aggregate.Year;
        model.PosterUrl = aggregate.PosterUrl;
        model.Description = aggregate.Description;
        model.ShortDescription = (string?)ShortDescription.GetValue(aggregate);
        model.RatingKp = aggregate.RatingKp;
        model.RatingImdb = aggregate.RatingImdb;
        model.UserRating = aggregate.UserRating;
        model.CountSeasons = aggregate.CountSeasons;
        model.CountEpisodes = aggregate.CountEpisodes;
        model.UserRatingsCount = aggregate.UserRatingsCount;

        var persons = await GetPersonsAsync(aggregate);

        ProcessCdns(aggregate, model);
        ProcessActors(aggregate, model, persons);
        ProcessDirectors(aggregate, model, persons);
        ProcessScreenwriters(aggregate, model, persons);

        await ProcessGenresAsync(aggregate, model);
        await ProcessCountriesAsync(aggregate, model);

        return model;
    }

    private async Task<IReadOnlyCollection<PersonModel>> GetPersonsAsync(Film aggregate)
    {
        var persons = aggregate.Directors
            .Concat(aggregate.Screenwriters)
            .Concat(aggregate.Actors.Select(a => a.Name))
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

    private void ProcessCdns(Film aggregate, FilmModel model)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.CdnList.RemoveAll(x => aggregate.CdnList.All(m => x.Name != m.Name));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newCdns = aggregate.CdnList
            .Where(x => model.CdnList.All(m => x.Name != m.Name))
            .Select(c => new CdnModel { Name = c.Name })
            .ToArray();

        foreach (var cdnModel in model.CdnList)
        {
            ProcessCdn(aggregate.CdnList.First(c => c.Name == cdnModel.Name), cdnModel);
        }

        foreach (var cdnModel in newCdns)
        {
            ProcessCdn(aggregate.CdnList.First(c => c.Name == cdnModel.Name), cdnModel);
        }

        model.CdnList.AddRange(newCdns);
    }

    private static void ProcessCdn(Cdn valueObject, CdnModel model)
    {
        model.Quality = valueObject.Quality;
        model.Url = valueObject.Url;
    }

    private async Task ProcessCountriesAsync(Film aggregate, FilmModel model)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.Countries.RemoveAll(x => aggregate.Countries.All(m => m != x.Name));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newCountries = aggregate.Countries
            .Where(x => model.Countries.All(m => m.Name != x))
            .ToArray();

        // Если новые записи есть
        if (newCountries.Length != 0)
        {
            // Запрашиваем все существующие модели EF для новых записей
            var databaseCountries = await context.Countries
                .Where(x => newCountries.Any(g => g == x.Name))
                .ToArrayAsync();

            // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
            model.Countries.AddRange(databaseCountries);

            // Определяем, какие модели не были получены из EF, для этих новых записей создаем новые модели EF
            var notDatabaseCountries = newCountries
                .Where(c => databaseCountries.All(d => d.Name != c))
                .Select(c => new CountryModel { Name = c });

            // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
            model.Countries.AddRange(notDatabaseCountries);
        }
    }

    private static void ProcessActors(Film aggregate, FilmModel model, IEnumerable<PersonModel> persons)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.Actors.RemoveAll(x => aggregate.Actors.All(m => m.Name != x.Person.Name));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newActors = aggregate.Actors
            .Where(x => model.Actors.All(m => m.Person.Name != x.Name))
            .ToArray();

        // Если новых записей нет
        if (newActors.Length == 0) return;

        // Получаем модели EF для новых записей
        var newPersons = persons
            .Where(x => newActors.Select(a => a.Name).Any(g => g == x.Name))
            .Select(p => new FilmActorModel
                { Person = p, Description = newActors.First(a => a.Name == p.Name).Description });

        // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
        model.Actors.AddRange(newPersons);
    }

    private async Task ProcessGenresAsync(Film aggregate, FilmModel model)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.Genres.RemoveAll(x => aggregate.Genres.All(m => m != x.Name));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newGenres = aggregate.Genres
            .Where(x => model.Genres.All(m => m.Name != x))
            .ToArray();

        // Если новые записи есть
        if (newGenres.Length != 0)
        {
            // Запрашиваем все существующие модели EF для новых записей
            var databaseGenres = await context.Genres
                .Where(x => newGenres.Any(g => g == x.Name))
                .ToArrayAsync();

            // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
            model.Genres.AddRange(databaseGenres);

            // Определяем, какие модели не были получены из EF, для этих новых записей создаем новые модели EF
            var notDatabaseGenres = newGenres
                .Where(c => databaseGenres.All(d => d.Name != c))
                .Select(c => new GenreModel { Name = c });

            // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
            model.Genres.AddRange(notDatabaseGenres);
        }
    }

    private static void ProcessDirectors(Film aggregate, FilmModel model, IEnumerable<PersonModel> persons)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.Directors.RemoveAll(x => aggregate.Directors.All(m => m != x.Name));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newDirectors = aggregate.Directors
            .Where(x => model.Directors.All(m => m.Name != x))
            .ToArray();

        // Если новых записей нет
        if (newDirectors.Length == 0) return;

        // Получаем модели EF для новых записей
        var newPersons = persons.Where(x => newDirectors.Any(g => g == x.Name));

        // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
        model.Directors.AddRange(newPersons);
    }

    private static void ProcessScreenwriters(Film aggregate, FilmModel model, IEnumerable<PersonModel> persons)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.Screenwriters.RemoveAll(x => aggregate.Screenwriters.All(m => m != x.Name));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newScreenwriters = aggregate.Screenwriters
            .Where(x => model.Screenwriters.All(m => m.Name != x))
            .ToArray();

        // Если новых записей нет
        if (newScreenwriters.Length == 0) return;

        // Получаем модели EF для новых записей
        var newPersons = persons.Where(x => newScreenwriters.Any(g => x.Name == g));

        // Добавляем полученные модели EF в коллекцию обрабатываемой модели EF
        model.Screenwriters.AddRange(newPersons);
    }
}