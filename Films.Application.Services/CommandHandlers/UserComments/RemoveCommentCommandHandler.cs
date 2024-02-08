using Films.Application.Abstractions.Commands.UserComments;
using Films.Application.Abstractions.Commands.UserComments.Exceptions;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.UserComments;

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