using Films.Domain.Playlists;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Extensions;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Playlists;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

internal class PlaylistModelMapper(ApplicationDbContext context) : IModelMapperUnit<PlaylistModel, Playlist>
{
    public async Task<PlaylistModel> MapAsync(Playlist aggregate)
    {
        var model = context.Playlists
            .LoadDependencies()
            .FirstOrDefault(x => x.Id == aggregate.Id) ?? new PlaylistModel { Id = aggregate.Id };

        model.Name = aggregate.Name;
        model.Description = aggregate.Description;
        model.Updated = aggregate.Updated;
        model.PosterUrl = aggregate.PosterUrl;

        ProcessFilms(aggregate, model);
        await ProcessGenresAsync(aggregate, model);

        return model;
    }

    private static void ProcessFilms(Playlist aggregate, PlaylistModel model)
    {
        model.Films.RemoveAll(x => aggregate.Films.All(m => m != x.FilmId));
        
        var newFilms = aggregate.Films
            .Where(x => model.Films.All(m => m.FilmId != x))
            .Select(x => new PlaylistFilmModel { FilmId = x });
        
        model.Films.AddRange(newFilms);
    }

    private async Task ProcessGenresAsync(Playlist aggregate, PlaylistModel model)
    {
        model.Genres.RemoveAll(x => aggregate.Genres.All(m => m != x.Name));

        var newGenres = aggregate.Genres
            .Where(x => model.Genres.All(m => m.Name != x))
            .ToArray();

        if (newGenres.Length != 0)
        {
            var databaseGenres = await context.Genres
                .Where(x => newGenres.Any(g => g == x.Name))
                .ToArrayAsync();

            model.Genres.AddRange(databaseGenres);
        }
    }
}