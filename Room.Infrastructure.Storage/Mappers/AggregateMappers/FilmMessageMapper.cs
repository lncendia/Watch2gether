using System.Runtime.CompilerServices;
using Room.Domain.Abstractions;
using Room.Domain.Messages.FilmMessages;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Mappers.StaticMethods;
using Room.Infrastructure.Storage.Models.FilmRooms;
using Room.Infrastructure.Storage.Models.Messages;

namespace Room.Infrastructure.Storage.Mappers.AggregateMappers;

internal class FilmMessageMapper : IAggregateMapperUnit<FilmMessage, MessageModel<FilmRoomModel>>
{
    public FilmMessage Map(MessageModel<FilmRoomModel> model)
    {
        var room = (FilmMessage)RuntimeHelpers.GetUninitializedObject(typeof(FilmMessage));
        MessageFields.Text.SetValue(room, model.Text);
        MessageFields.CreatedAt.SetValue(room, model.CreatedAt);
        MessageFields.UserId.SetValue(room, model.UserId);
        MessageFields.RoomId.SetValue(room, model.RoomId);
        IdFields.AggregateId.SetValue(room, model.Id);
        IdFields.DomainEvents.SetValue(room, new List<IDomainEvent>());
        return room;
    }
}