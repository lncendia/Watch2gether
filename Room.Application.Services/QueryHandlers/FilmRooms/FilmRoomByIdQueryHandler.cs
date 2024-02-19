using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Queries.DTOs.FilmRoom;
using Room.Application.Abstractions.Queries.FilmRooms;
using Room.Application.Services.Common;
using Room.Application.Services.Mappers;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.QueryHandlers.FilmRooms;

/// <summary>
/// Обработчик запроса на получение комнаты
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class FilmRoomByIdQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<FilmRoomByIdQuery, FilmRoomDto>
{
    public async Task<FilmRoomDto> Handle(FilmRoomByIdQuery request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetFilmRoomFromCache(request.Id, unitOfWork);

        // Преобразовываем комнату в DTO и возвращаем
        return FilmRoomMapper.Map(room);
    }
}