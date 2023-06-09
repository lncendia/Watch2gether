using Overoom.Domain.Playlist.Entities;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.Playlists;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class PlaylistMapper : IAggregateMapperUnit<Playlist, PlaylistModel>
{
    public Playlist Map(PlaylistModel model)
    {
        var playlist = new Playlist(model.Films.Select(x=>x.Id), model.PosterUri, model.Name, model.Description);
        IdFields.AggregateId.SetValue(playlist, model.Id);
        return playlist;
    }
}