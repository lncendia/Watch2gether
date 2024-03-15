using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Abstractions.DTOs.FilmRooms;
using Room.Application.Services.Common;
using Room.Application.Services.Mappers;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Rooms.FilmRooms.Entities;

namespace Room.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды на подключение к комнате
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class ConnectCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache)
    : IRequestHandler<ConnectCommand, FilmRoomDto>
{
    public async Task<FilmRoomDto> Handle(ConnectCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetFilmRoomFromCache(request.RoomId, unitOfWork);

        // Подключаем пользователя
        room.Connect(new FilmViewer
        {
            Id = request.Viewer.Id,
            Allows = request.Viewer.Allows,
            PhotoUrl = request.Viewer.PhotoUrl,
            Username = request.Viewer.Nickname
        });

        // Обновляем комнату в репозитории
        await unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);

        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();

        // Преобразовываем комнату в DTO и возвращаем
        return FilmRoomMapper.Map(room);
    }
}