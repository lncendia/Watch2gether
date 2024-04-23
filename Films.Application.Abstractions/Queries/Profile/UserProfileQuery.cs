using Films.Application.Abstractions.DTOs.Profile;
using MediatR;

namespace Films.Application.Abstractions.Queries.Profile;

public class UserProfileQuery : IRequest<UserProfileDto>
{
    public required Guid Id { get; init; }
}