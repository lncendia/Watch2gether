using Microsoft.EntityFrameworkCore;
using Room.Domain.Films.Entities;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Extensions;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.Film;

namespace Room.Infrastructure.Storage.Mappers.ModelMappers;

internal class FilmModelMapper(ApplicationDbContext context) : IModelMapperUnit<FilmModel, Film>
{
    public async Task<FilmModel> MapAsync(Film entity)
    {
        var film = await context.Films
            .LoadDependencies()
            .FirstOrDefaultAsync(x => x.Id == entity.Id) ?? new FilmModel { Id = entity.Id };

        film.Type = entity.Type;
        film.Title = entity.Title;
        film.Year = entity.Year;
        film.PosterUrl = entity.PosterUrl;
        film.Description = entity.Description;

        ProcessCdns(entity, film);

        return film;
    }

    private static void ProcessCdns(Film entity, FilmModel film)
    {
        // Удаляем эти записи из коллекции в модели EF
        film.CdnList.RemoveAll(g => entity.CdnList.All(m => g.Name != m.Name));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newCdns = entity.CdnList
            .Where(x => film.CdnList.All(m => x.Name != m.Name))
            .Select(c => new CdnModel { Name = c.Name, Url = c.Url })
            .ToArray();

        foreach (var cdnModel in film.CdnList)
        {
            cdnModel.Url = entity.CdnList.First(c => c.Name == cdnModel.Name).Url;
        }

        film.CdnList.AddRange(newCdns);
    }
}