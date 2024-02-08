using MediatR;
using Room.Application.Abstractions.DTOs.FilmRoom;
using Room.Application.Abstractions.DTOs.YoutubeRoom;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на создание комнаты
/// </summary>
public class CreateRoomCommand : IRequest<YoutubeRoomDto>
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public required Guid UserId { get; init; }
    
    /// <summary>
    /// Флаг, открыта ли комната
    /// </summary>
    public required bool IsOpen { get; init; }
    
    /// <summary>
    /// Ссылка на видео
    /// </summary>
    public required Uri VideoUrl { get; init; }
    
    /// <summary>
    /// Флаг, есть ли у пользователей доступ к изменение видео
    /// </summary>
    public required bool VideoAccess { get; init; }
}