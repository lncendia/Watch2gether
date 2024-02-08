using Films.Application.Abstractions.Queries.Users;
using Films.Application.Abstractions.Queries.Users.DTOs;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Films.Entities;
using Films.Domain.Films.Specifications;
using Films.Domain.Ordering;
using Films.Domain.Ratings.Entities;
using Films.Domain.Ratings.Ordering;
using Films.Domain.Ratings.Ordering.Visitor;
using Films.Domain.Ratings.Specifications;
using MediatR;

namespace Films.Application.Services.QueryHandlers.Users;

public class UserRatingsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UserRatingsQuery, (IReadOnlyCollection<RatingDto> retings, int count)>
{
    public async Task<(IReadOnlyCollection<RatingDto> retings, int count)> Handle(UserRatingsQuery request,
        CancellationToken cancellationToken)
    {
        var specification = new RatingByUserSpecification(request.Id);
        var order = new DescendingOrder<Rating, IRatingSortingVisitor>(new RatingOrderByDate());

        var ratings =
            await unitOfWork.RatingRepository.Value.FindAsync(specification, order, request.Skip, request.Take);

        if (ratings.Count == 0) return ([], 0);

        var filmSpecification = new FilmByIdsSpecification(ratings.Select(r => r.FilmId));

        var films = await unitOfWork.FilmRepository.Value.FindAsync(filmSpecification);

        var ratingsDtos = ratings.Select(r => Map(r, films.First(f => f.Id == r.FilmId))).ToArray();

        var count = await unitOfWork.RatingRepository.Value.CountAsync(specification);

        return (ratingsDtos, count);
    }

    private static RatingDto Map(Rating rating, Film film) => new()
    {
        FilmId = film.Id,
        Name = film.Title,
        Year = film.Year,
        PosterUrl = film.PosterUrl,
        Score = rating.Score
    };
}