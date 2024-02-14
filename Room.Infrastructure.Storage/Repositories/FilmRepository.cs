using Microsoft.EntityFrameworkCore;
using Room.Domain.Abstractions.Repositories;
using Room.Domain.Films.Entities;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Extensions;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.Film;

namespace Room.Infrastructure.Storage.Repositories;

public class FilmRepository(
    ApplicationDbContext context,
    IAggregateMapperUnit<Film, FilmModel> aggregateMapper,
    IModelMapperUnit<FilmModel, Film> modelMapper)
    : IFilmRepository
{
    public async Task AddAsync(Film entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        var film = await modelMapper.MapAsync(entity);
        await context.AddAsync(film);
    }

    public async Task UpdateAsync(Film entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        await modelMapper.MapAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        context.Remove(context.Films.First(film => film.Id == id));
        return Task.CompletedTask;
    }

    public async Task<Film?> GetAsync(Guid id)
    {
        var film = await context.Films
            .LoadDependencies()
            .AsNoTracking()
            .FirstOrDefaultAsync(userModel => userModel.Id == id);

        return film == null ? null : aggregateMapper.Map(film);
    }
}