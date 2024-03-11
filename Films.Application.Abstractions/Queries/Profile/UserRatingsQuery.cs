using Films.Application.Abstractions.DTOs.Profile;
using MediatR;

namespace Films.Application.Abstractions.Queries.Profile;

public class UserRatingsQuery : IRequest<(IReadOnlyCollection<UserRatingDto> ratings, int count)>
{
    public required Guid Id { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}