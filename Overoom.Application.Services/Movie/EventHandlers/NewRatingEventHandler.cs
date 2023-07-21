using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Overoom.Application.Abstractions.Common.Exceptions;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Ratings;
using Overoom.Domain.Ratings.Entities;
using Overoom.Domain.Ratings.Events;
using Overoom.Domain.Ratings.Specifications;
using Overoom.Domain.Ratings.Specifications.Visitor;
using Overoom.Domain.Specifications;

namespace Overoom.Application.Services.Movie.EventHandlers;

public class NewRatingEventHandler : INotificationHandler<NewRatingEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _memoryCache;

    public NewRatingEventHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    {
        _unitOfWork = unitOfWork;
        _memoryCache = memoryCache;
    }

    public async Task Handle(NewRatingEvent notification, CancellationToken cancellationToken)
    {
        if (!_memoryCache.TryGetValue(notification.Rating.FilmId, out Film? film))
        {
            film = await _unitOfWork.FilmRepository.Value.GetAsync(notification.Rating.FilmId);
            if (film == null) throw new FilmNotFoundException();
            _memoryCache.Set(notification.Rating.FilmId, film,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        }
        else
        {
            if (film == null) throw new FilmNotFoundException();
        }

        var userSpec = new RatingByUserSpecification(notification.Rating.UserId!.Value);
        var filmSpec = new RatingByFilmSpecification(notification.Rating.FilmId);
        var ratingList = await
            _unitOfWork.RatingRepository.Value.FindAsync(
                new AndSpecification<Rating, IRatingSpecificationVisitor>(userSpec, filmSpec));
        var rating = ratingList.FirstOrDefault();
        film.AddRating(notification.Rating, rating);

        if (rating != null) await _unitOfWork.RatingRepository.Value.DeleteAsync(rating.Id);
        await _unitOfWork.FilmRepository.Value.UpdateAsync(film);
        _memoryCache.Remove(film.Id);
    }
}