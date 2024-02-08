using MediatR;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Application.Abstractions.DTOs.YoutubeRoom;
using Room.Application.Services.Mappers;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Rooms.FilmRoom.Entities;
using Room.Domain.Rooms.YoutubeRoom.Entities;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды создания комнаты
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class CreateRoomCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateRoomCommand, YoutubeRoomDto>
{
    public async Task<YoutubeRoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);

        // Если пользователь не найден - вызываем исключение
        if (user == null) throw new UserNotFoundException();

        // Создаем комнату
        var room = new YoutubeRoom(user, request.VideoUrl, request.IsOpen, request.VideoAccess);

        // Добавляем комнату в репозиторий
        await unitOfWork.YoutubeRoomRepository.Value.AddAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
        
        // Преобразовываем комнату в DTO и возвращаем
        return await YoutubeRoomMapper.MapAsync(room, unitOfWork);
    }
}