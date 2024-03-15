using Films.Domain.Rooms.FilmRooms.Events;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.FilmRooms;

namespace Films.Application.Services.EventHandlers.FilmRooms;

public class FilmRoomUserLeavedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<FilmRoomUserLeavedDomainEvent>
{
    public async Task Handle(FilmRoomUserLeavedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new FilmRoomViewerLeavedIntegrationEvent
        {
            RoomId = notification.Room.Id,
            ViewerId = notification.UserId
        }, context => context.SetRoutingKey(notification.Room.ServerId.ToString()), cancellationToken);
    }
}