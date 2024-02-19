using Films.Application.Abstractions.Commands.Rooms.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Commands.Rooms.FilmRooms;

/// <summary>
/// Команда на подключение к комнате с фильмом
/// </summary>
public class ConnectFilmRoomCommand : IRequest<RoomServerDto>
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