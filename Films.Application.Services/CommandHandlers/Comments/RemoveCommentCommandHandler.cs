using Films.Application.Abstractions.Commands.Comments;
using Films.Application.Abstractions.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.Comments;

public class RemoveCommentCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RemoveCommentCommand>
{
    public async Task Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await unitOfWork.CommentRepository.Value.GetAsync(request.CommentId);
        if (comment == null) throw new CommentNotFoundException();
        if (comment.UserId != request.UserId) throw new CommentNotBelongToUserException();
        await unitOfWork.CommentRepository.Value.DeleteAsync(comment.Id);
        await unitOfWork.SaveChangesAsync();
    }
}