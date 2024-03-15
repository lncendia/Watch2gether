using MediatR;
using Room.Application.Abstractions.DTOs.Messages;

namespace Room.Application.Abstractions.Queries.FilmRooms;

/// <summary>
/// Запрос для получения сообщений комнаты с фильмом.
/// </summary>
public class FilmRoomMessagesQuery : IRequest<(IReadOnlyCollection<MessageDto> messages, int count)>
{
    /// <summary>
    /// Идентификатор комнаты.
    /// </summary>
    public required Guid RoomId { get; init; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public required Guid ViewerId { get; init; }

    /// <summary>
    /// Идентификатор сообщения, с которого начинать выборку.
    /// </summary>
    public Guid? FromMessageId { get; init; }

    /// <summary>
    /// Количество сообщений для выборки.
    /// </summary>
    public required int Count { get; init; }
}