using Films.Application.Abstractions.Common.Exceptions;
using MediatR;
using Films.Application.Abstractions.Queries.Films;
using Films.Application.Abstractions.Queries.Films.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Films.Entities;
using Films.Domain.Ratings.Entities;
using Films.Domain.Ratings.Specifications;
using Films.Domain.Ratings.Specifications.Visitor;
using Films.Domain.Specifications;
using Films.Domain.Users.Entities;

namespace Films.Application.Services.Queries.Films;

public class FilmByIdQueryHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    : IRequestHandler<FilmByIdQuery, FilmDto>
{
    public async Task<FilmDto> Handle(FilmByIdQuery request, CancellationToken cancellationToken)
    {
        // Получаем фильм по его идентификатору 
        var film = await GetFilmAsync(request.Id);

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
        Type = film.Type,
        Name = film.Title,
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
        InWatchlist = user?.Watchlist.Any(f => f.FilmId == film.Id)
    };

    /// <summary>
    /// Асинхронно получает фильм из кэша или базы данных по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор фильма.</param>
    /// <returns>Объект фильма.</returns>
    private async Task<Film> GetFilmAsync(Guid id)
    {
        // Проверяем, есть ли фильм в кэше 
        if (memoryCache.TryGetValue(id, out Film? film)) return film!;

        // Если фильм не найден в кэше, получаем его из репозитория
        film = await unitOfWork.FilmRepository.Value.GetAsync(id);

        // Если фильм не найден, выбрасываем исключение 
        if (film == null) throw new FilmNotFoundException();

        // Добавляем фильм в кэш с временем жизни 5 минут 
        memoryCache.Set(id, film, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

        // Возвращаем найденный фильм 
        return film;
    }
}