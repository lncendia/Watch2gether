using Films.Domain.Rooms.YoutubeRooms.Events;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;

namespace Films.Application.Services.EventHandlers.YoutubeRooms;

public class YoutubeRoomDeletedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<YoutubeRoomDeletedDomainEvent>
{
    public async Task Handle(YoutubeRoomDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new YoutubeRoomDeletedIntegrationEvent { Id = notification.Room.Id },
            context => context.SetRoutingKey(notification.Room.ServerId.ToString()), cancellationToken);
    }
}