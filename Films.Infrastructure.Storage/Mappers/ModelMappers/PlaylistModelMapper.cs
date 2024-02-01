using Films.Domain.Playlists.Entities;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Playlist;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

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


        var newFilms = entity.Films.Where(x => playlist.Films.All(m => m.FilmId != x));
        var removeFilms = playlist.Films.Where(x => entity.Films.All(m => m != x.FilmId));
        playlist.Films.AddRange(newFilms.Select(x => new PlaylistFilmModel { FilmId = x }));
        context.PlaylistFilms.RemoveRange(removeFilms);

        var newGenres = entity.Genres.Where(x => playlist.Genres.All(m => !string.Equals(m.Name, x, StringComparison.CurrentCultureIgnoreCase)));
        var removeGenres = playlist.Genres.Where(x => entity.Genres.All(m => !string.Equals(m, x.Name, StringComparison.CurrentCultureIgnoreCase)));
        var databaseGenres = context.Genres.Where(x => newGenres.Any(g => g.Equals(x.Name, StringComparison.CurrentCultureIgnoreCase)));
        
        playlist.Genres.AddRange(databaseGenres);
        playlist.Genres.RemoveAll(g => removeGenres.Contains(g));

        return playlist;
    }
}