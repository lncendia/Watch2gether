using Films.Application.Abstractions.DTOs.Profile;
using Films.Application.Abstractions.Queries.Profile;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Films;
using Films.Domain.Films.Specifications;
using Films.Domain.Ordering;
using Films.Domain.Ratings;
using Films.Domain.Ratings.Ordering;
using Films.Domain.Ratings.Ordering.Visitor;
using Films.Domain.Ratings.Specifications;
using MediatR;

namespace Films.Application.Services.QueryHandlers.Profile;

public class UserRatingsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UserRatingsQuery, (IReadOnlyCollection<UserRatingDto> retings, int count)>
{
    public async Task<(IReadOnlyCollection<UserRatingDto> retings, int count)> Handle(UserRatingsQuery request,
        CancellationToken cancellationToken)
    {
        var specification = new RatingByUserSpecification(request.Id);
        var order = new DescendingOrder<Rating, IRatingSortingVisitor>(new RatingOrderByDate());

        var ratings =
            await unitOfWork.RatingRepository.Value.FindAsync(specification, order, request.Skip, request.Take);

        if (ratings.Count == 0) return ([], 0);

        var filmSpecification = new FilmsByIdsSpecification(ratings.Select(r => r.FilmId));

        var films = await unitOfWork.FilmRepository.Value.FindAsync(filmSpecification);

        var ratingsDtos = ratings.Select(r => Map(r, films.First(f => f.Id == r.FilmId))).ToArray();

        var count = await unitOfWork.RatingRepository.Value.CountAsync(specification);

        return (ratingsDtos, count);
    }

    private static UserRatingDto Map(Rating rating, Film film) => new()
    {
        Id = film.Id,
        Title = film.Title,
        Year = film.Year,
        PosterUrl = film.PosterUrl,
        Score = rating.Score,
        RatingKp = film.RatingKp,
        RatingImdb = film.RatingImdb
    };
}