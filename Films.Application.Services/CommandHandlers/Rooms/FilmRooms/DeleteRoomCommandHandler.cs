using Films.Application.Abstractions.Commands.Rooms.FilmRooms;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.Rooms.FilmRooms;

/// <summary>
/// Обработчик команды на удаление комнаты с фильмом
/// </summary>
public class DeleteRoomCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteRoomCommand>
{
    public async Task Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        // Удаляем комнату из репозитория
        await unitOfWork.FilmRoomRepository.Value.DeleteAsync(request.RoomId);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}