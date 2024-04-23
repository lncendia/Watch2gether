using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;
using Room.Domain.Rooms.YoutubeRooms.Events;

namespace Room.Application.Services.EventHandlers.YoutubeRooms;

public class YoutubeRoomViewerKickedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<YoutubeRoomViewerKickedDomainEvent>
{
    public async Task Handle(YoutubeRoomViewerKickedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new YoutubeRoomViewerKickedIntegrationEvent
        {
            RoomId = notification.Room.Id,
            ViewerId = notification.ViewerId
        }, cancellationToken);
    }
}