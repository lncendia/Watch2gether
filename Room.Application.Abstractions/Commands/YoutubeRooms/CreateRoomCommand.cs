using MediatR;
using Room.Application.Abstractions.Commands.BaseRooms;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на создание комнаты
/// </summary>
public class CreateRoomCommand : IRequest
{
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Зритель
    /// </summary>
    public required ViewerData Owner { get; init; }

    /// <summary>
    /// Флаг, есть ли у пользователей доступ к изменение видео
    /// </summary>
    public required bool VideoAccess { get; init; }
}