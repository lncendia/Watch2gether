using MediatR;
using Room.Application.Abstractions.DTOs.FilmRoom;

namespace Room.Application.Abstractions.Commands.FilmRooms;

/// <summary>
/// Команда на подключение к комнате
/// </summary>
public class ConnectCommand : IRequest<FilmRoomDto>
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid RoomId { get; init; }

    /// <summary>
    /// Проверочный код
    /// </summary>
    public string? Code { get; init; }
}