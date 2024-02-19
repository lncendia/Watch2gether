using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Queries.DTOs.YoutubeRoom;
using Room.Application.Abstractions.Queries.YoutubeRooms;
using Room.Application.Services.Common;
using Room.Application.Services.Mappers;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.QueryHandlers.YoutubeRooms;

/// <summary>
/// Обработчик запроса на получение комнаты
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class YoutubeRoomByIdQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<YoutubeRoomByIdQuery, YoutubeRoomDto>
{
    public async Task<YoutubeRoomDto> Handle(YoutubeRoomByIdQuery request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetYoutubeRoomFromCache(request.Id, unitOfWork);

        // Преобразовываем комнату в DTO и возвращаем
        return YoutubeRoomMapper.Map(room);
    }
}