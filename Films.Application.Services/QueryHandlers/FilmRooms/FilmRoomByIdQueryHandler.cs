using Films.Application.Abstractions.DTOs.Rooms;
using Films.Application.Abstractions.Exceptions;
using Films.Application.Abstractions.Queries.FilmRooms;
using Films.Application.Services.Common;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Films;
using Films.Domain.Ratings;
using Films.Domain.Ratings.Specifications;
using Films.Domain.Ratings.Specifications.Visitor;
using Films.Domain.Rooms.FilmRooms;
using Films.Domain.Specifications;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Films.Application.Services.QueryHandlers.FilmRooms;

public class FilmRoomByIdQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache)
    : IRequestHandler<FilmRoomByIdQuery, FilmRoomDto>
{
    public async Task<FilmRoomDto> Handle(FilmRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await unitOfWork.FilmRoomRepository.Value.GetAsync(request.Id);
        if (room == null) throw new RoomNotFoundException();

        var film = await cache.TryGetFilmFromCacheAsync(room.FilmId, unitOfWork);

        // Если не указан идентификатор пользователя, возвращаем отображение фильма без рейтинга и пользователя 
        if (!request.UserId.HasValue) return Map(room, film, null, null);

        // Создаем спецификации для получения рейтинга пользователя для данного фильма 
        var userSpec = new RatingByUserSpecification(request.UserId.Value);
        var filmSpec = new RatingByFilmSpecification(room.FilmId);

        // Получаем список рейтингов, удовлетворяющих спецификациям 
        var ratingList = await unitOfWork.RatingRepository.Value.FindAsync(
            new AndSpecification<Rating, IRatingSpecificationVisitor>(userSpec, filmSpec));

        // Получаем первый рейтинг из списка 
        var rating = ratingList.FirstOrDefault();

        return Map(room, film, rating, request.UserId);
    }

    private static FilmRoomDto Map(FilmRoom room, Film film, Rating? rating, Guid? userId) => new()
    {
        Title = film.Title,
        PosterUrl = film.PosterUrl,
        Year = film.Year,
        RatingKp = film.RatingKp,
        RatingImdb = film.RatingImdb,
        UserRating = film.UserRating,
        Description = film.Description,
        IsSerial = film.IsSerial,
        Id = room.Id,
        ViewersCount = room.Viewers.Count,
        FilmId = film.Id,
        IsCodeNeeded = room.Viewers.All(v => v != userId) && !string.IsNullOrEmpty(room.Code),
        UserRatingsCount = film.UserRatingsCount,
        UserScore = rating?.Score,
        IsPrivate = !string.IsNullOrEmpty(room.Code)
    };
}