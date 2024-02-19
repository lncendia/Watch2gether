using MediatR;

namespace Room.Application.Abstractions.Commands.BaseRooms;

/// <summary>
/// Базовый класс для команды комнаты
/// </summary>
public abstract class RoomCommand : IRequest
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public required Guid ViewerId { get; init; }
    
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid RoomId { get; init; }
}