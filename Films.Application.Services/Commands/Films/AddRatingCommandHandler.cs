using Films.Application.Abstractions.Commands.Films;
using Films.Application.Abstractions.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Films.Entities;
using Films.Domain.Ratings.Entities;

namespace Films.Application.Services.Commands.Films;

public class AddRatingCommandHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache) : IRequestHandler<AddRatingCommand>
{
    public async Task Handle(AddRatingCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя по его идентификатору 
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();

        // Создаем новый рейтинг с указанным идентификатором фильма, идентификатором пользователя и оценкой 
        var rating = new Rating(await GetFilmAsync(request.Id), user, request.Score);

        // Добавляем рейтинг в репозиторий 
        await unitOfWork.RatingRepository.Value.AddAsync(rating);
        await unitOfWork.SaveChangesAsync();
    }
    
    /// <summary>
    /// Асинхронно получает фильм из кэша или базы данных по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор фильма.</param>
    /// <returns>Объект фильма.</returns>
    private async Task<Film> GetFilmAsync(Guid id)
    {
        // Проверяем, есть ли фильм в кэше 
        if (!memoryCache.TryGetValue(id, out Film? film))
        {
            // Если фильм не найден в кэше, получаем его из репозитория
            film = await unitOfWork.FilmRepository.Value.GetAsync(id);

            // Если фильм не найден, выбрасываем исключение 
            if (film == null) throw new FilmNotFoundException();

            // Добавляем фильм в кэш с временем жизни 5 минут 
            memoryCache.Set(id, film, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        }
        else
        {
            // Если фильм в кэше равен null, выбрасываем исключение 
            if (film == null) throw new FilmNotFoundException();
        }

        // Возвращаем найденный фильм 
        return film;
    }
}