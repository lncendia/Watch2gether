using Films.Application.Abstractions.Queries.Users.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.Users;

public class UserProfileQuery : IRequest<UserProfileDto>
{
    public required Guid Id { get; init; }
}