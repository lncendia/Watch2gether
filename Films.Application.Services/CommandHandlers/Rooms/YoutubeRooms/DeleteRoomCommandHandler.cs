using Films.Application.Abstractions.Commands.Rooms.YoutubeRooms;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.Rooms.YoutubeRooms;

/// <summary>
/// Обработчик команды на удаление комнаты ютуб
/// </summary>
public class DeleteRoomCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteRoomCommand>
{
    public async Task Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        // Удаляем комнату из репозитория
        await unitOfWork.YoutubeRoomRepository.Value.DeleteAsync(request.RoomId);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}