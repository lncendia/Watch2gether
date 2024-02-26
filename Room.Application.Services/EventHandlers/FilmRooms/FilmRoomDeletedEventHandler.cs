using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.FilmRooms;
using Room.Domain.FilmRooms.Events;

namespace Room.Application.Services.EventHandlers.FilmRooms;

public class FilmRoomDeletedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<FilmRoomDeletedDomainEvent>
{
    public async Task Handle(FilmRoomDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new FilmRoomDeletedIntegrationEvent { Id = notification.Room.Id },
            cancellationToken);
    }
}