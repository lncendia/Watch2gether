using Films.Application.Abstractions.Queries.Comments.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.Comments;

public class FilmCommentsQuery : IRequest<(IReadOnlyCollection<CommentDto> comments, long count)>
{
    public required Guid FilmId { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}