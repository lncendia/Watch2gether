using Films.Application.Abstractions.Queries.Profile.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.Profile;

public class UserProfileQuery : IRequest<UserProfileDto>
{
    public required Guid Id { get; init; }
}