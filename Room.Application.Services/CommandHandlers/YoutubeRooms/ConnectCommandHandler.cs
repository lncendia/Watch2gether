using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Abstractions.Queries.DTOs.YoutubeRoom;
using Room.Application.Services.Common;
using Room.Application.Services.Mappers;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.YoutubeRooms.Entities;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на подключение к комнате
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class ConnectCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<ConnectCommand, YoutubeRoomDto>
{
    public async Task<YoutubeRoomDto> Handle(ConnectCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetYoutubeRoomFromCache(request.RoomId, unitOfWork);

        // Подключаем пользователя
        room.Connect(new YoutubeViewer
        {
            Id = request.Viewer.Id,
            Allows = request.Viewer.Allows,
            PhotoUrl = request.Viewer.PhotoUrl,
            Nickname = request.Viewer.Nickname
        });
              
        // Обновляем комнату в репозитории
        await unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();

        // Преобразовываем комнату в DTO и возвращаем
        return YoutubeRoomMapper.Map(room);
    }
}