using Films.Application.Abstractions.Commands.Ratings;
using Films.Application.Abstractions.Exceptions;
using Films.Application.Services.Common;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Ratings;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Films.Application.Services.CommandHandlers.Ratings;

public class AddRatingCommandHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache) : IRequestHandler<AddRatingCommand>
{
    public async Task Handle(AddRatingCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя по его идентификатору 
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();

        var film = await memoryCache.TryGetFilmFromCacheAsync(request.FilmId, unitOfWork);
        
        // Создаем новый рейтинг с указанным идентификатором фильма, идентификатором пользователя и оценкой 
        var rating = new Rating(film, user, request.Score);

        // Добавляем рейтинг в репозиторий 
        await unitOfWork.RatingRepository.Value.AddAsync(rating);
        await unitOfWork.SaveChangesAsync();
    }
}