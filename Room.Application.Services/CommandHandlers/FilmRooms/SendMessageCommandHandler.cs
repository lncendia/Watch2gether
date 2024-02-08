using MediatR;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды на отправку сообщения
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class SendMessageCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<SendMessageCommand>
{
    public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату из репозитория
        var room = await unitOfWork.FilmRoomRepository.Value.GetAsync(request.RoomId);
        
        // Если комната не найдена - вызываем исключение
        if (room == null) throw new RoomNotFoundException();

        // Отправляем сообщения
        room.SendMessage(request.UserId, request.Message);
              
        // Обновляем комнату в репозитории
        await unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}