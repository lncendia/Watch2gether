using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Rooms.FilmRoom.Entities;
using Room.Domain.Rooms.YoutubeRoom.Entities;

namespace Room.Application.Services.Common;

public static class RoomExtensions
{
    /// <summary>
    /// Асинхронно получает комнату из кэша или базы данных по его идентификатору.
    /// </summary>
    /// <param name="memoryCache">Интерфейс кеширования объектов в памяти</param>
    /// <param name="id">Идентификатор комнаты</param>
    /// <param name="unitOfWork">Единица работы</param>
    /// <returns>Объект фильма.</returns>
    public static async Task<FilmRoom> TryGetFilmRoomFromCache(this IMemoryCache memoryCache, Guid id,
        IUnitOfWork unitOfWork)
    {
        // Проверяем, есть ли комната в кэше 
        if (memoryCache.TryGetValue(id, out FilmRoom? room)) return room!;

        // Если комната не найдена в кэше, получаем его из репозитория
        room = await unitOfWork.FilmRoomRepository.Value.GetAsync(id);

        // Если комната не найдена, выбрасываем исключение 
        if (room == null) throw new RoomNotFoundException();

        // Добавляем комнату в кэш с скользящим временем жизни 10 минут 
        memoryCache.Set(id, room, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1)));

        // Возвращаем найденную комнату
        return room;
    }
    
    /// <summary>
    /// Асинхронно получает комнату из кэша или базы данных по его идентификатору.
    /// </summary>
    /// <param name="memoryCache">Интерфейс кеширования объектов в памяти</param>
    /// <param name="id">Идентификатор комнаты</param>
    /// <param name="unitOfWork">Единица работы</param>
    /// <returns>Объект фильма.</returns>
    public static async Task<YoutubeRoom> TryGetYoutubeRoomFromCache(this IMemoryCache memoryCache, Guid id,
        IUnitOfWork unitOfWork)
    {
        // Проверяем, есть ли комната в кэше 
        if (memoryCache.TryGetValue(id, out YoutubeRoom? room)) return room!;

        // Если комната не найдена в кэше, получаем его из репозитория
        room = await unitOfWork.YoutubeRoomRepository.Value.GetAsync(id);

        // Если комната не найдена, выбрасываем исключение 
        if (room == null) throw new RoomNotFoundException();

        // Добавляем комнату в кэш с скользящим временем жизни 10 минут 
        memoryCache.Set(id, room, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1)));

        // Возвращаем найденную комнату
        return room;
    }
}