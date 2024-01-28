using Films.Application.Abstractions.Common.Exceptions;
using MediatR;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Films.Specifications;
using Films.Domain.Ordering;
using Films.Domain.Ratings.Entities;
using Films.Domain.Ratings.Events;
using Films.Domain.Ratings.Ordering;
using Films.Domain.Ratings.Ordering.Visitor;
using Films.Domain.Ratings.Specifications;

namespace Films.Application.Services.EventHandlers;

public class NewRatingUserEventHandler(IUnitOfWork unitOfWork) : INotificationHandler<NewRatingEvent>
{
    public async Task Handle(NewRatingEvent notification, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.Value.GetAsync(notification.UserId);
        if (user == null) throw new UserNotFoundException();
        var spec = new RatingByUserSpecification(user.Id);
        var orderScore = new RatingOrderByScore();
        var orderDate = new RatingOrderByDate();
        var order = new DescendingOrder<Rating, IRatingSortingVisitor>(
            new ThenByOrder<Rating, IRatingSortingVisitor>(orderDate, orderScore));
        var ratings =
            await unitOfWork.RatingRepository.Value.FindAsync(spec, order, 0, 10);
        var films = await unitOfWork.FilmRepository.Value.FindAsync(
            new FilmByIdsSpecification(ratings.Select(x => x.FilmId)));
        var genres = ratings.SelectMany(x => films.First(f => f.Id == x.FilmId).Genres).GroupBy(g => g)
            .OrderByDescending(genre => genre.Count()).Select(x => x.Key).Take(5);
        user.UpdateGenres(genres);
        await unitOfWork.UserRepository.Value.UpdateAsync(user);
    }
}