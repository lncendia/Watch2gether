using Films.Domain.Playlists.Entities;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Playlist;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

// ReSharper disable EntityFramework.NPlusOne.IncompleteDataUsage
// ReSharper disable EntityFramework.NPlusOne.IncompleteDataQuery
internal class PlaylistModelMapper(ApplicationDbContext context) : IModelMapperUnit<PlaylistModel, Playlist>
{
    public async Task<PlaylistModel> MapAsync(Playlist entity)
    {
        var playlist = context.Playlists.FirstOrDefault(x => x.Id == entity.Id);

        if (playlist != null)
        {
            await context.Entry(playlist).Collection(x => x.Films).LoadAsync();
            await context.Entry(playlist).Collection(x => x.Genres).LoadAsync();
        }
        else playlist = new PlaylistModel { Id = entity.Id };

        playlist.Name = entity.Name;
        playlist.Description = entity.Description;
        playlist.Updated = entity.Updated;
        playlist.PosterUrl = entity.PosterUrl;

        ProcessFilms(entity, playlist);
        await ProcessGenresAsync(entity, playlist);

        return playlist;
    }

    private void ProcessFilms(Playlist entity, PlaylistModel playlist)
    {
        var newFilms = entity.Films
            .Where(x => playlist.Films.All(m => m.FilmId != x))
            .Select(x => new PlaylistFilmModel { FilmId = x });
        
        var removeFilms = playlist.Films
            .Where(x => entity.Films.All(m => m != x.FilmId));
        
        playlist.Films.AddRange(newFilms);
        
        context.PlaylistFilms.RemoveRange(removeFilms);
    }
    
    private async Task ProcessGenresAsync(Playlist entity, PlaylistModel playlist)
    {
        var removeGenres = playlist.Genres
            .Where(x => entity.Genres.All(m => m != x.Name));
        
        playlist.Genres.RemoveAll(g => removeGenres.Contains(g));
        
        var newGenres = entity.Genres
            .Where(x => playlist.Genres.All(m => m.Name != x))
            .ToArray();
        
        if (newGenres.Length != 0)
        {
            var databaseGenres = await context.Genres
                .Where(x => newGenres.Any(g => g == x.Name))
                .ToArrayAsync();
            
            playlist.Genres.AddRange(databaseGenres);
        }
    }
}