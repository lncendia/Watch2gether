using Films.Domain.Rooms.FilmRooms.Events;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.BaseRooms;
using Overoom.IntegrationEvents.Rooms.FilmRooms;

namespace Films.Application.Services.EventHandlers.FilmRooms;

public class FilmRoomUserConnectedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<FilmRoomUserConnectedDomainEvent>
{
    public async Task Handle(FilmRoomUserConnectedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new FilmRoomViewerConnectedIntegrationEvent
        {
            RoomId = notification.Room.Id,
            Viewer = new Viewer
            {
                Id = notification.User.Id,
                PhotoUrl = notification.User.PhotoUrl,
                Name = notification.User.Username,
                Beep = notification.User.Allows.Beep,
                Scream = notification.User.Allows.Scream,
                Change = notification.User.Allows.Change
            }
        }, context => context.SetRoutingKey(notification.Room.ServerId.ToString()), cancellationToken);
    }
}