using Films.Domain.Abstractions.Interfaces;
using MediatR;
using Films.Domain.Films.Specifications;
using Films.Domain.Ordering;
using Films.Domain.Ratings;
using Films.Domain.Ratings.Events;
using Films.Domain.Ratings.Ordering;
using Films.Domain.Ratings.Ordering.Visitor;
using Films.Domain.Ratings.Specifications;

namespace Films.Application.Services.EventHandlers;

public class NewRatingUserEventHandler(IUnitOfWork unitOfWork) : INotificationHandler<NewRatingDomainEvent>
{
    public async Task Handle(NewRatingDomainEvent notification, CancellationToken cancellationToken)
    {
        var specification = new RatingByUserSpecification(notification.User.Id);
        var orderScore = new RatingOrderByScore();
        var orderDate = new RatingOrderByDate();
        var order = new DescendingOrder<Rating, IRatingSortingVisitor>(
            new ThenByOrder<Rating, IRatingSortingVisitor>(orderDate, orderScore));
        
        var ratings =
            await unitOfWork.RatingRepository.Value.FindAsync(specification, order, 0, 10);

        var filmsSpecification = new FilmsByIdsSpecification(ratings.Select(x => x.FilmId));
        var films = await unitOfWork.FilmRepository.Value.FindAsync(filmsSpecification);
        var genres = ratings
            .SelectMany(x => films.First(f => f.Id == x.FilmId).Genres)
            .GroupBy(g => g)
            .OrderByDescending(genre => genre.Count())
            .Select(x => x.Key)
            .Take(5);
        
        notification.User.UpdateGenres(genres);
        await unitOfWork.UserRepository.Value.UpdateAsync(notification.User);
    }
}