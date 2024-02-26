using Films.Domain.Rooms.YoutubeRooms.Events;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;

namespace Films.Application.Services.EventHandlers.Rooms.YoutubeRooms;

public class YoutubeRoomUserLeavedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<YoutubeRoomUserLeavedDomainEvent>
{
    public async Task Handle(YoutubeRoomUserLeavedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new YoutubeRoomViewerLeavedIntegrationEvent
        {
            RoomId = notification.Room.Id,
            ViewerId = notification.UserId
        }, context => context.SetRoutingKey(notification.Room.ServerId.ToString()), cancellationToken);
    }
}