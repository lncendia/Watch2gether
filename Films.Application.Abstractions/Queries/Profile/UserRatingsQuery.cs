using Films.Application.Abstractions.DTOs.Common;
using Films.Application.Abstractions.DTOs.Profile;
using MediatR;

namespace Films.Application.Abstractions.Queries.Profile;

public class UserRatingsQuery : IRequest<ListDto<UserRatingDto>>
{
    public required Guid Id { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}