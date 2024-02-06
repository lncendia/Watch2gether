using Films.Application.Abstractions.Queries.Users.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.Users;

public class UserRatingsQuery : IRequest<(IReadOnlyCollection<RatingDto> ratings, int count)>
{
    public required Guid Id { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}