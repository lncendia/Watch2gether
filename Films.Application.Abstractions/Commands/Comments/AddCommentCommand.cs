using MediatR;

namespace Films.Application.Abstractions.Commands.Comments;

public class AddCommentCommand : IRequest<Guid>
{
    public required Guid FilmId { get; init; }
    public required Guid UserId { get; init; }
    public required string Text { get; init; }
}