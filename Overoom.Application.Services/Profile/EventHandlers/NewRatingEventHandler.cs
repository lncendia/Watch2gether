using MediatR;
using Overoom.Application.Abstractions.Common.Exceptions;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films.Specifications;
using Overoom.Domain.Ordering;
using Overoom.Domain.Ratings.Entities;
using Overoom.Domain.Ratings.Events;
using Overoom.Domain.Ratings.Ordering;
using Overoom.Domain.Ratings.Ordering.Visitor;
using Overoom.Domain.Ratings.Specifications;

namespace Overoom.Application.Services.Profile.EventHandlers;

public class NewRatingEventHandler : INotificationHandler<NewRatingEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public NewRatingEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(NewRatingEvent notification, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(notification.Rating.UserId!.Value);
        if (user == null) throw new UserNotFoundException();
        var spec = new RatingByUserSpecification(user.Id);
        var orderScore = new RatingOrderByScore();
        var orderDate = new RatingOrderByDate();
        var order = new DescendingOrder<Rating, IRatingSortingVisitor>(
            new ThenByOrder<Rating, IRatingSortingVisitor>(orderDate, orderScore));
        var ratings =
            await _unitOfWork.RatingRepository.Value.FindAsync(spec, order, 0, 10);
        var films = await _unitOfWork.FilmRepository.Value.FindAsync(
            new FilmByIdsSpecification(ratings.Select(x => x.FilmId)));
        var genres = ratings.SelectMany(x => films.First(f => f.Id == x.FilmId).FilmTags.Genres).GroupBy(g => g)
            .OrderByDescending(genre => genre.Count()).Select(x => x.Key).Take(5);
        user.UpdateGenres(genres);
        await _unitOfWork.UserRepository.Value.UpdateAsync(user);
    }
}