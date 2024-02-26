using Films.Application.Abstractions.Commands.Comments;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Services.Common;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Comments;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Films.Application.Services.CommandHandlers.Comments;

public class AddCommentCommandHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    : IRequestHandler<AddCommentCommand, Guid>
{
    public async Task<Guid> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();
        
        var film = await memoryCache.TryGetFilmFromCacheAsync(request.FilmId, unitOfWork);
        
        var comment = new Comment(film, request.UserId, request.Text);
        
        await unitOfWork.CommentRepository.Value.AddAsync(comment);
        await unitOfWork.SaveChangesAsync();
        return comment.Id;
    }
}