using System.Runtime.CompilerServices;
using Room.Domain.Abstractions;
using Room.Domain.Messages.YoutubeMessages;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Mappers.StaticMethods;
using Room.Infrastructure.Storage.Models.YoutubeRooms;
using Room.Infrastructure.Storage.Models.Messages;

namespace Room.Infrastructure.Storage.Mappers.AggregateMappers;

internal class YoutubeMessageMapper : IAggregateMapperUnit<YoutubeMessage, MessageModel<YoutubeRoomModel>>
{
    public YoutubeMessage Map(MessageModel<YoutubeRoomModel> model)
    {
        var room = (YoutubeMessage)RuntimeHelpers.GetUninitializedObject(typeof(YoutubeMessage));
        MessageFields.Text.SetValue(room, model.Text);
        MessageFields.CreatedAt.SetValue(room, model.CreatedAt);
        MessageFields.UserId.SetValue(room, model.UserId);
        MessageFields.RoomId.SetValue(room, model.RoomId);
        IdFields.AggregateId.SetValue(room, model.Id);
        IdFields.DomainEvents.SetValue(room, new List<IDomainEvent>());
        return room;
    }
}