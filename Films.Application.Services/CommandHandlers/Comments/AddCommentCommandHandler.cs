using Films.Application.Abstractions.Commands.Comments;
using Films.Application.Abstractions.DTOs.Comments;
using Films.Application.Abstractions.Exceptions;
using Films.Application.Services.Extensions;
using Films.Application.Services.Mappers.Comments;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Comments;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Films.Application.Services.CommandHandlers.Comments;

public class AddCommentCommandHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    : IRequestHandler<AddCommentCommand, CommentDto>
{
    public async Task<CommentDto> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();
        
        var film = await memoryCache.TryGetFilmFromCacheAsync(request.FilmId, unitOfWork);
        
        var comment = new Comment(film, user, request.Text);
        
        await unitOfWork.CommentRepository.Value.AddAsync(comment);
        await unitOfWork.SaveChangesAsync();
        return Mapper.Map(comment, user);
    }
}