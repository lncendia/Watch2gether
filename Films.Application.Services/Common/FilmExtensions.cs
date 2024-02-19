using Films.Application.Abstractions.Common.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Films;
using Microsoft.Extensions.Caching.Memory;

namespace Films.Application.Services.Common;

public static class FilmExtensions
{
    /// <summary>
    /// Асинхронно получает фильм из кэша или базы данных по его идентификатору.
    /// </summary>
    /// <param name="memoryCache">Интерфейс кеширования объектов в памяти</param>
    /// <param name="id">Идентификатор фильма.</param>
    /// <param name="unitOfWork">Единица работы</param>
    /// <returns>Объект фильма.</returns>
    public static async Task<Film> TryGetFilmFromCacheAsync(this IMemoryCache memoryCache, Guid id, IUnitOfWork unitOfWork)
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