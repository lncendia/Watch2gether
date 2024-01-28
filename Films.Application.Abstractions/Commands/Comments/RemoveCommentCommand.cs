using MediatR;

namespace Films.Application.Abstractions.Commands.Comments;

public class RemoveCommentCommand : IRequest
{
    public required Guid UserId { get; init; }
    public required Guid CommentId { get; init; }
}