using Overoom.Domain.Playlists.Entities;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.Playlist;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class PlaylistMapper : IAggregateMapperUnit<Playlist, PlaylistModel>
{
    public Playlist Map(PlaylistModel model)
    {
        var playlist = new Playlist(model.Films.Select(x => x.FilmId), model.PosterUri, model.Name, model.Description);
        IdFields.AggregateId.SetValue(playlist, model.Id);
        return playlist;
    }
}