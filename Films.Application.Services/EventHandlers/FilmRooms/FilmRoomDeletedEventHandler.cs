using Films.Domain.Rooms.FilmRooms.Events;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.FilmRooms;

namespace Films.Application.Services.EventHandlers.FilmRooms;

public class FilmRoomDeletedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<FilmRoomDeletedDomainEvent>
{
    public async Task Handle(FilmRoomDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new FilmRoomDeletedIntegrationEvent { Id = notification.Room.Id },
            context => context.SetRoutingKey(notification.Room.ServerId.ToString()), cancellationToken);
    }
}