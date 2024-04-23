using System.Reflection;
using Films.Domain.Playlists;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Mappers.StaticMethods;
using Films.Infrastructure.Storage.Models.Playlists;

namespace Films.Infrastructure.Storage.Mappers.AggregateMappers;

internal class PlaylistMapper : IAggregateMapperUnit<Playlist, PlaylistModel>
{
    private static readonly Type PlaylistType = typeof(Playlist);

    private static readonly FieldInfo Films =
        PlaylistType.GetField("_films", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo Genres =
        PlaylistType.GetField("_genres", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public Playlist Map(PlaylistModel model)
    {
        var playlist = new Playlist
        {
            Name = model.Name,
            Description = model.Description,
            PosterUrl = model.PosterUrl
        };
        IdFields.AggregateId.SetValue(playlist, model.Id);
        Films.SetValue(playlist, model.Films.Select(x => x.FilmId).ToList());
        Genres.SetValue(playlist, model.Genres.Select(x => x.Name).ToList());
        return playlist;
    }
}