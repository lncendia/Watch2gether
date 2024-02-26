using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;
using Room.Domain.YoutubeRooms.Events;

namespace Room.Application.Services.EventHandlers.YoutubeRooms;

public class YoutubeRoomDeletedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<YoutubeRoomDeletedDomainEvent>
{
    public async Task Handle(YoutubeRoomDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new YoutubeRoomDeletedIntegrationEvent { Id = notification.Room.Id },
            cancellationToken);
    }
}