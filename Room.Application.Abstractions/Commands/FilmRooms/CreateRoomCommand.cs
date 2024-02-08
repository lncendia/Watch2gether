using MediatR;
using Room.Application.Abstractions.DTOs.FilmRoom;

namespace Room.Application.Abstractions.Commands.FilmRooms;

/// <summary>
/// Команда на создание комнаты
/// </summary>
public class CreateRoomCommand : IRequest<FilmRoomDto>
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