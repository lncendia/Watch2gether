using Films.Application.Abstractions.DTOs.Profile;
using Films.Application.Abstractions.Exceptions;
using Films.Application.Abstractions.Queries.Profile;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Films;
using Films.Domain.Films.Specifications;
using MediatR;

namespace Films.Application.Services.QueryHandlers.Profile;

public class UserProfileQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<UserProfileQuery, UserProfileDto>
{
    public async Task<UserProfileDto> Handle(UserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.Id);

        if (user == null) throw new UserNotFoundException();

        var filmSpecification =
            new FilmsByIdsSpecification(user.History.Select(h => h.FilmId).Union(user.Watchlist.Select(w => w.FilmId)));

        var films = await unitOfWork.FilmRepository.Value.FindAsync(filmSpecification);

        return new UserProfileDto
        {
            Username = user.Username,
            PhotoUrl = user.PhotoUrl,
            Allows = user.Allows,
            Genres = user.Genres,
            History = user.History
                .Select(n => Map(films.First(f => f.Id == n.FilmId)))
                .ToArray(),
            Watchlist = user.Watchlist
                .Select(n => Map(films.First(f => f.Id == n.FilmId)))
                .ToArray()
        };
    }

    private static UserFilmDto Map(Film film) => new()
    {
        Id = film.Id,
        Title = film.Title,
        Year = film.Year,
        PosterUrl = film.PosterUrl,
        RatingKp = film.RatingKp,
        RatingImdb = film.RatingImdb
    };
}