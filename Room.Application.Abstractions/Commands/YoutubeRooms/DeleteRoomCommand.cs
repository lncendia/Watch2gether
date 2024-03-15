using MediatR;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на удаление комнаты ютуб
/// </summary>
public class DeleteRoomCommand : IRequest
{
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid Id { get; init; }
}