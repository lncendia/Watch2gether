using Films.Application.Abstractions.Queries.Comments;
using Films.Application.Abstractions.Queries.Comments.DTOs;
using MediatR;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Comments.Entities;
using Films.Domain.Comments.Ordering;
using Films.Domain.Comments.Ordering.Visitor;
using Films.Domain.Comments.Specifications;
using Films.Domain.Ordering;
using Films.Domain.Users.Entities;
using Films.Domain.Users.Specifications;

namespace Films.Application.Services.Queries.Comments;

/// <summary>
/// Обработчик запроса для получения комментариев к фильму.
/// </summary>
public class FilmCommentsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<FilmCommentsQuery, (IReadOnlyCollection<CommentDto> comments, long count)>
{
    /// <summary>
    /// Обрабатывает запрос на комментарии к фильму и возвращает соответствующие комментарии.
    /// </summary>
    /// <param name="request">Запрос на комментарии к фильму.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция DTO комментариев.</returns>
    public async Task<(IReadOnlyCollection<CommentDto> comments, long count)> Handle(FilmCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var specification = new FilmCommentsSpecification(request.FilmId);
        var sorting = new DescendingOrder<Comment, ICommentSortingVisitor>(new CommentOrderByDate());
        var comments =
            await unitOfWork.CommentRepository.Value.FindAsync(specification, sorting, request.Skip, request.Take);

        if (comments.Count == 0) return (Array.Empty<CommentDto>(), 0);

        var spec = new UsersByIdsSpecification(comments.Select(x => x.UserId));
        var users = await unitOfWork.UserRepository.Value.FindAsync(spec);

        var count = await unitOfWork.CommentRepository.Value.CountAsync(specification);

        var commentDtos = comments.Select(c => Map(c, users.First(u => u.Id == c.UserId))).ToArray();

        return (commentDtos, count);
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