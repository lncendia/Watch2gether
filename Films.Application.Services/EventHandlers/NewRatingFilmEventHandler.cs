using Films.Application.Abstractions.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Films.Entities;
using Films.Domain.Ratings.Entities;
using Films.Domain.Ratings.Events;
using Films.Domain.Ratings.Specifications;
using Films.Domain.Ratings.Specifications.Visitor;
using Films.Domain.Specifications;

namespace Films.Application.Services.EventHandlers;

public class NewRatingFilmEventHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    : INotificationHandler<NewRatingEvent>
{
    public async Task Handle(NewRatingEvent notification, CancellationToken cancellationToken)
    {
        var film = await GetFilmAsync(notification.Rating.FilmId);

        var userSpec = new RatingByUserSpecification(notification.Rating.UserId!.Value);
        var filmSpec = new RatingByFilmSpecification(notification.Rating.FilmId);
        var ratingList = await
            unitOfWork.RatingRepository.Value.FindAsync(
                new AndSpecification<Rating, IRatingSpecificationVisitor>(userSpec, filmSpec));
        var oldRating = ratingList.FirstOrDefault();
        film.AddRating(notification.Rating, oldRating);

        if (oldRating != null) await unitOfWork.RatingRepository.Value.DeleteAsync(oldRating.Id);
        await unitOfWork.FilmRepository.Value.UpdateAsync(film);
    }
    
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