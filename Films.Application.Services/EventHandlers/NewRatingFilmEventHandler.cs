using Films.Application.Abstractions.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Films.Entities;
using Films.Domain.Ratings.Entities;
using Films.Domain.Ratings.Events;
using Films.Domain.Ratings.Specifications;
using Films.Domain.Ratings.Specifications.Visitor;
using Films.Domain.Specifications;

namespace Films.Application.Services.EventHandlers;

public class NewRatingFilmEventHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    : INotificationHandler<NewRatingEvent>
{
    public async Task Handle(NewRatingEvent notification, CancellationToken cancellationToken)
    {
        if (!memoryCache.TryGetValue(notification.FilmId, out Film? film))
        {
            film = await unitOfWork.FilmRepository.Value.GetAsync(notification.FilmId);
            if (film == null) throw new FilmNotFoundException();
            memoryCache.Set(notification.FilmId, film,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        }
        else
        {
            if (film == null) throw new FilmNotFoundException();
        }

        var userSpec = new RatingByUserSpecification(notification.UserId);
        var filmSpec = new RatingByFilmSpecification(notification.FilmId);
        var ratingList = await
            unitOfWork.RatingRepository.Value.FindAsync(
                new AndSpecification<Rating, IRatingSpecificationVisitor>(userSpec, filmSpec));
        var oldRating = ratingList.FirstOrDefault();
        film.AddRating(notification.Rating, oldRating);

        if (oldRating != null) await unitOfWork.RatingRepository.Value.DeleteAsync(oldRating.Id);
        await unitOfWork.FilmRepository.Value.UpdateAsync(film);
    }
}