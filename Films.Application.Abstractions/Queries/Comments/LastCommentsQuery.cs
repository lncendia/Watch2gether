using Films.Application.Abstractions.DTOs.Comments;
using MediatR;

namespace Films.Application.Abstractions.Queries.Comments;

public class LastCommentsQuery : IRequest<IReadOnlyCollection<CommentDto>>
{
    public required int Take { get; init; }
}