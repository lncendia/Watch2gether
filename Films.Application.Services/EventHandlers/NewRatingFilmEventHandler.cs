using MediatR;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Ratings.Entities;
using Films.Domain.Ratings.Events;
using Films.Domain.Ratings.Specifications;
using Films.Domain.Ratings.Specifications.Visitor;
using Films.Domain.Specifications;

namespace Films.Application.Services.EventHandlers;

public class NewRatingFilmEventHandler(IUnitOfWork unitOfWork) : INotificationHandler<NewRatingEvent>
{
    public async Task Handle(NewRatingEvent notification, CancellationToken cancellationToken)
    {
        var userSpec = new RatingByUserSpecification(notification.Rating.UserId!.Value);
        var filmSpec = new RatingByFilmSpecification(notification.Rating.FilmId);
        var ratingList = await
            unitOfWork.RatingRepository.Value.FindAsync(
                new AndSpecification<Rating, IRatingSpecificationVisitor>(userSpec, filmSpec));
        var oldRating = ratingList.FirstOrDefault();
        notification.Film.AddRating(notification.Rating, oldRating);

        if (oldRating != null) await unitOfWork.RatingRepository.Value.DeleteAsync(oldRating.Id);
        await unitOfWork.FilmRepository.Value.UpdateAsync(notification.Film);
    }
}