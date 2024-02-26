using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.FilmRooms;
using Room.Domain.FilmRooms.Events;

namespace Room.Application.Services.EventHandlers.FilmRooms;

public class FilmRoomViewerKickedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<FilmRoomViewerKickedDomainEvent>
{
    public async Task Handle(FilmRoomViewerKickedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new FilmRoomViewerKickedIntegrationEvent
        {
            RoomId = notification.Room.Id,
            ViewerId = notification.ViewerId
        }, cancellationToken);
    }
}