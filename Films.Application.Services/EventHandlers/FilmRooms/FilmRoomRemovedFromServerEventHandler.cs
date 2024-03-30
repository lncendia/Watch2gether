using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.FilmRooms.Events;
using MediatR;

namespace Films.Application.Services.EventHandlers.FilmRooms;

public class FilmRoomRemovedFromServerEventHandler(IUnitOfWork unitOfWork)
    : INotificationHandler<FilmRoomDeDomainEvent>
{
    public async Task Handle(FilmRoomCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        notification.Server.RoomsCount += 1;
        await unitOfWork.ServerRepository.Value.UpdateAsync(notification.Server);
    }
}