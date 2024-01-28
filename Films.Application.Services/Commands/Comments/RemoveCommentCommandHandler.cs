using Films.Application.Abstractions.Commands.Comments;
using Films.Application.Abstractions.Commands.Comments.Exceptions;
using Films.Application.Abstractions.Common.Exceptions;
using MediatR;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;

namespace Films.Application.Services.Commands.Comments;

public class RemoveCommentCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RemoveCommentCommand>
{
    public required Guid FilmId { get; init; }
    public required Guid Id { get; init; }
    public required Guid CommentId { get; init; }
    public async Task Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await unitOfWork.CommentRepository.Value.GetAsync(request.CommentId);
        if (comment == null) throw new CommentNotFoundException();
        if (comment.UserId != request.UserId) throw new CommentNotBelongToUserException();
        await unitOfWork.CommentRepository.Value.DeleteAsync(comment.Id);
        await unitOfWork.SaveChangesAsync();
    }
}