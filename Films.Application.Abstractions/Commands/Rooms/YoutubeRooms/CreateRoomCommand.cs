using Films.Application.Abstractions.Commands.Rooms.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Commands.Rooms.YoutubeRooms;

/// <summary>
/// Команда на создание комнаты ютуб
/// </summary>
public class CreateRoomCommand : IRequest<RoomServerDto>
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
    /// Флаг, есть ли у пользователей доступ к изменение видео
    /// </summary>
    public required bool VideoAccess { get; init; }
}