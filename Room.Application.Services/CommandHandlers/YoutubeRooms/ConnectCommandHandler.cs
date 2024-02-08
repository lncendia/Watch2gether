using MediatR;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Application.Abstractions.DTOs.YoutubeRoom;
using Room.Application.Services.Mappers;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на подключение к комнате
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class ConnectCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ConnectCommand, YoutubeRoomDto>
{
    public async Task<YoutubeRoomDto> Handle(ConnectCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату из репозитория
        var room = await unitOfWork.YoutubeRoomRepository.Value.GetAsync(request.RoomId);

        // Если комната не найдена - вызываем исключение
        if (room == null) throw new RoomNotFoundException();

        // Если зритель уже подключен к комнате
        if (room.Viewers.Any(v => v.UserId == request.UserId))
        {
            // Устанавливаем, что пользователь онлайн
            room.SetOnline(request.UserId, true);
        }
        else
        {
            // Получаем пользователя
            var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);

            // Если пользователь не найден - вызываем исключение
            if (user == null) throw new UserNotFoundException();

            // Подключаем пользователя
            room.Connect(user, request.Code);
        }
              
        // Обновляем комнату в репозитории
        await unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();

        // Преобразовываем комнату в DTO и возвращаем
        return await YoutubeRoomMapper.MapAsync(room, unitOfWork);
    }
}