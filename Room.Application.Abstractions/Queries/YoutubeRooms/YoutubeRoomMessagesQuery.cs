using MediatR;
using Room.Application.Abstractions.DTOs.Messages;

namespace Room.Application.Abstractions.Queries.YoutubeRooms;

/// <summary>
/// Запрос для получения сообщений комнаты ютуб.
/// </summary>
public class YoutubeRoomMessagesQuery : IRequest<(IReadOnlyCollection<MessageDto> messages, int count)>
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
    public required Guid? FromMessageId { get; init; }

    /// <summary>
    /// Количество сообщений для выборки.
    /// </summary>
    public required int Count { get; init; }
}