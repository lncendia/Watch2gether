using System.Reflection;
using Overoom.Domain.Playlists.Entities;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.Playlist;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class PlaylistMapper : IAggregateMapperUnit<Playlist, PlaylistModel>
{
    private static readonly Type PlaylistType = typeof(Playlist);

    private static readonly FieldInfo Films =
        PlaylistType.GetField("_films", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public Playlist Map(PlaylistModel model)
    {
        var playlist = new Playlist(model.PosterUri, model.Name, model.Description, model.Genres.Select(x => x.Name));
        IdFields.AggregateId.SetValue(playlist, model.Id);
        Films.SetValue(playlist, model.Films.Select(x=>x.FilmId).ToList());
        return playlist;
    }
}