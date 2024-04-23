using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;
using Room.Domain.Rooms.YoutubeRooms.Events;

namespace Room.Application.Services.EventHandlers.YoutubeRooms;

public class YoutubeRoomViewerLeavedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<YoutubeRoomViewerLeavedDomainEvent>
{
    public async Task Handle(YoutubeRoomViewerLeavedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new YoutubeRoomViewerLeavedIntegrationEvent
        {
            RoomId = notification.Room.Id,
            ViewerId = notification.ViewerId
        }, cancellationToken);
    }
}