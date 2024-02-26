using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.YoutubeRooms.Events;
using MediatR;

namespace Films.Application.Services.EventHandlers.Rooms.YoutubeRooms;

public class YoutubeRoomAddedToServerEventHandler(IUnitOfWork unitOfWork)
    : INotificationHandler<YoutubeRoomCreatedDomainEvent>
{
    public async Task Handle(YoutubeRoomCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        notification.Server.RoomsCount += 1;
        await unitOfWork.ServerRepository.Value.UpdateAsync(notification.Server);
    }
}