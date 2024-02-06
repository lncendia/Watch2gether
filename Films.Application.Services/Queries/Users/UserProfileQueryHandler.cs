using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Queries.Users;
using Films.Application.Abstractions.Queries.Users.DTOs;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using MediatR;

namespace Films.Application.Services.Queries.Users;

public class UserProfileQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<UserProfileQuery, UserProfileDto>
{
    public async Task<UserProfileDto> Handle(UserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.Id);

        if (user == null) throw new UserNotFoundException();

        return new UserProfileDto
        {
            UserName = user.UserName,
            PhotoUrl = user.PhotoUrl,
            Allows = user.Allows,
            Genres = user.Genres
        };
    }
}