using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.FilmRooms;
using Room.Domain.Rooms.FilmRooms.Events;

namespace Room.Application.Services.EventHandlers.FilmRooms;

public class FilmRoomViewerLeavedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<FilmRoomViewerLeavedDomainEvent>
{
    public async Task Handle(FilmRoomViewerLeavedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new FilmRoomViewerLeavedIntegrationEvent
        {
            RoomId = notification.Room.Id,
            ViewerId = notification.ViewerId
        }, cancellationToken);
    }
}