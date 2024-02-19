using Films.Application.Abstractions.Queries.Comments;
using Films.Application.Abstractions.Queries.Comments.DTOs;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Comments;
using Films.Domain.Comments.Ordering;
using Films.Domain.Comments.Ordering.Visitor;
using Films.Domain.Ordering;
using Films.Domain.Users;
using Films.Domain.Users.Specifications;
using MediatR;

namespace Films.Application.Services.QueryHandlers.Comments;

/// <summary>
/// Обработчик запроса для получения комментариев к фильму.
/// </summary>
public class LastCommentsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<LastCommentsQuery, IReadOnlyCollection<CommentDto>>
{
    /// <summary>
    /// Обрабатывает запрос на последние комментарии и возвращает соответствующие комментарии.
    /// </summary>
    /// <param name="request">Запрос на комментарии.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция DTO комментариев.</returns>
    public async Task<IReadOnlyCollection<CommentDto>> Handle(LastCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var comments = await unitOfWork.CommentRepository.Value.FindAsync(null,
            new DescendingOrder<Comment, ICommentSortingVisitor>(
                new CommentOrderByDate()), take: 20);

        var specification = new UserByIdsSpecification(comments.Select(x => x.UserId));

        var users = await unitOfWork.UserRepository.Value.FindAsync(specification);

        return comments.Select(x => Map(x, users.First(u => u.Id == x.UserId))).ToArray();
    }

    /// <summary>
    /// Преобразует сущность комментария и сущность пользователя в DTO комментария.
    /// </summary>
    /// <param name="comment">Сущность комментария.</param>
    /// <param name="user">Сущность пользователя.</param>
    /// <returns>DTO комментария.</returns>
    private static CommentDto Map(Comment comment, User user) => new()
    {
        Id = comment.Id,
        UserId = user.Id,
        Text = comment.Text,
        CreatedAt = comment.CreatedAt,
        Username = user.UserName,
        PhotoUrl = user.PhotoUrl
    };
}