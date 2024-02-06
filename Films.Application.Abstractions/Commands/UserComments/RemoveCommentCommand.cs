using MediatR;

namespace Films.Application.Abstractions.Commands.UserComments;

public class RemoveCommentCommand : IRequest
{
    public required Guid UserId { get; init; }
    public required Guid CommentId { get; init; }
}