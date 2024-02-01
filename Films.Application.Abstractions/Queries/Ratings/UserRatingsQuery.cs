using Films.Application.Abstractions.Queries.Ratings.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.Ratings;

public class UserRatingsQuery : IRequest<(IReadOnlyCollection<RatingDto> retings, long count)>
{
    public required Guid Id { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}