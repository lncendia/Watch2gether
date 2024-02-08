namespace Room.Application.Abstractions.Commands.Base;

/// <summary>
/// Базовый класс для команды комнаты направленной на какого-то зрителя
/// </summary>
public abstract class RoomTargetCommand : RoomCommand
{
    public required Guid TargetId { get; init; }
}