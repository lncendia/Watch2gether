using MediatR;

namespace Room.Application.Abstractions.Commands.FilmRooms;

/// <summary>
/// Команда на удаление комнаты с фильмом
/// </summary>
public class DeleteRoomCommand : IRequest
{
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid Id { get; init; }
}