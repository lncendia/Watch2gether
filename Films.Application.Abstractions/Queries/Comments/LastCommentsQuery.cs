using Films.Application.Abstractions.Queries.Comments.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.Comments;

public class LastCommentsQuery : IRequest<IReadOnlyCollection<CommentDto>>
{
    public required int Take { get; init; }
}