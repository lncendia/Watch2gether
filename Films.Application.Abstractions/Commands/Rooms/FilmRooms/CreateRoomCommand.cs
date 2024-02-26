using Films.Application.Abstractions.Commands.Rooms.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Commands.Rooms.FilmRooms;

/// <summary>
/// Команда на создание комнаты с фильмом
/// </summary>
public class CreateRoomCommand : IRequest<RoomServerDto>
{
    /// <summary>
    /// Идентификатор фильма
    /// </summary>
    public required Guid FilmId { get; init; }
    
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Имя CDN
    /// </summary>
    public required string CdnName { get; init; }
    
    /// <summary>
    /// Флаг, открыта ли комната
    /// </summary>
    public required bool IsOpen { get; init; }
}