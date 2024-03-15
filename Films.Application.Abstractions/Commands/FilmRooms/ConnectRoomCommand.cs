using Films.Application.Abstractions.DTOs.Rooms;
using MediatR;

namespace Films.Application.Abstractions.Commands.FilmRooms;

/// <summary>
/// Команда на подключение к комнате с фильмом
/// </summary>
public class ConnectRoomCommand : IRequest<RoomServerDto>
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