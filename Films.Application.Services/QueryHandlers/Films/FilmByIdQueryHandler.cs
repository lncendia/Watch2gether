using Films.Application.Abstractions.DTOs.Films;
using Films.Application.Abstractions.Exceptions;
using Films.Application.Abstractions.Queries.Films;
using Films.Application.Services.Extensions;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Films;
using Films.Domain.Ratings;
using Films.Domain.Ratings.Specifications;
using Films.Domain.Ratings.Specifications.Visitor;
using Films.Domain.Specifications;
using Films.Domain.Users;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Films.Application.Services.QueryHandlers.Films;

public class FilmByIdQueryHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    : IRequestHandler<FilmByIdQuery, FilmDto>
{
    public async Task<FilmDto> Handle(FilmByIdQuery request, CancellationToken cancellationToken)
    {
        // Получаем фильм по его идентификатору 
        var film = await memoryCache.TryGetFilmFromCacheAsync(request.Id, unitOfWork);

        Rating? rating = null;
        User? user = null;

        // Если не указан идентификатор пользователя, возвращаем отображение фильма без рейтинга и пользователя 
        if (!request.UserId.HasValue) return Map(film, rating, user);

        // Получаем пользователя по его идентификатору 
        user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId.Value);
        if (user == null) throw new UserNotFoundException();

        // Создаем спецификации для получения рейтинга пользователя для данного фильма 
        var userSpec = new RatingByUserSpecification(request.UserId.Value);
        var filmSpec = new RatingByFilmSpecification(request.Id);

        // Получаем список рейтингов, удовлетворяющих спецификациям 
        var ratingList = await unitOfWork.RatingRepository.Value.FindAsync(
            new AndSpecification<Rating, IRatingSpecificationVisitor>(userSpec, filmSpec));

        // Получаем первый рейтинг из списка 
        rating = ratingList.FirstOrDefault();

        // Возвращаем отображение фильма с рейтингом и пользователем 
        return Map(film, rating, user);
    }

    private static FilmDto Map(Film film, Rating? rating, User? user) => new()
    {
        Id = film.Id,
        Description = film.Description,
        IsSerial = film.IsSerial,
        Title = film.Title,
        PosterUrl = film.PosterUrl,
        Year = film.Year,
        RatingKp = film.RatingKp,
        RatingImdb = film.RatingImdb,
        UserRating = film.UserRating,
        UserRatingsCount = film.UserRatingsCount,
        CdnList = film.CdnList,
        Genres = film.Genres,
        Countries = film.Countries,
        Directors = film.Directors,
        ScreenWriters = film.Screenwriters,
        Actors = film.Actors,
        UserScore = rating?.Score,
        CountSeasons = film.CountSeasons,
        CountEpisodes = film.CountEpisodes,
        InWatchlist = user?.Watchlist.Any(f => f.FilmId == film.Id)
    };
}