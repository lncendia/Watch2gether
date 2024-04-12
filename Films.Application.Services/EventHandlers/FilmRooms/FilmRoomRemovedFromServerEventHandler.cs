using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.FilmRooms.Events;
using MediatR;

namespace Films.Application.Services.EventHandlers.FilmRooms;

public class FilmRoomRemovedFromServerEventHandler(IUnitOfWork unitOfWork)
    : INotificationHandler<FilmRoomDeletedDomainEvent>
{
    public async Task Handle(FilmRoomDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var server = await unitOfWork.ServerRepository.Value.GetAsync(notification.FilmRoom.ServerId);
        if (server == null) return;
        server.RoomsCount -= 1;
        await unitOfWork.ServerRepository.Value.UpdateAsync(server);
    }
}