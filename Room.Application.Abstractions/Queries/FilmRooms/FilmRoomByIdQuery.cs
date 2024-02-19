using MediatR;
using Room.Application.Abstractions.Queries.DTOs.FilmRoom;

namespace Room.Application.Abstractions.Queries.FilmRooms;

/// <summary>
/// Запрос на получение комнаты
/// </summary>
public class FilmRoomByIdQuery : IRequest<FilmRoomDto>
{
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid Id { get; init; }
}