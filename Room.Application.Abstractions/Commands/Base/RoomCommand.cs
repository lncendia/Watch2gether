using MediatR;

namespace Room.Application.Abstractions.Commands.Base;

/// <summary>
/// Базовый класс для команды комнаты
/// </summary>
public abstract class RoomCommand : IRequest
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public required Guid UserId { get; init; }
    
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid RoomId { get; init; }
}