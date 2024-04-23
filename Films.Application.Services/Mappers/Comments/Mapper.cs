using Films.Application.Abstractions.DTOs.Comments;
using Films.Domain.Comments;
using Films.Domain.Users;

namespace Films.Application.Services.Mappers.Comments;

internal static class Mapper
{
    /// <summary>
    /// Преобразует сущность комментария и сущность пользователя в DTO комментария.
    /// </summary>
    /// <param name="comment">Сущность комментария.</param>
    /// <param name="user">Сущность пользователя.</param>
    /// <returns>DTO комментария.</returns>
    internal static CommentDto Map(Comment comment, User user) => new()
    {
        Id = comment.Id,
        UserId = user.Id,
        Text = comment.Text,
        CreatedAt = comment.CreatedAt,
        Username = user.Username,
        PhotoUrl = user.PhotoUrl
    };
}