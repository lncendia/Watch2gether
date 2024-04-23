using Films.Application.Abstractions.DTOs.Comments;
using Films.Application.Abstractions.Queries.Comments;
using Films.Application.Services.Mappers.Comments;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Comments;
using Films.Domain.Comments.Ordering;
using Films.Domain.Comments.Ordering.Visitor;
using Films.Domain.Ordering;
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

        return comments.Select(x => Mapper.Map(x, users.First(u => u.Id == x.UserId))).ToArray();
    }
}