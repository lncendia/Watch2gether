using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Application.Abstractions.DTOs.FilmRoom;
using Room.Application.Services.Common;
using Room.Application.Services.Mappers;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды на подключение к комнате
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class ConnectCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<ConnectCommand, FilmRoomDto>
{
    public async Task<FilmRoomDto> Handle(ConnectCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetFilmRoomFromCache(request.RoomId, unitOfWork);

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
        await unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();

        // Преобразовываем комнату в DTO и возвращаем
        return await FilmRoomMapper.MapAsync(room, unitOfWork);
    }
}