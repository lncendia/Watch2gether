using Films.Application.Abstractions.Commands.Films;
using Films.Application.Abstractions.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Films.Entities;

namespace Films.Application.Services.Commands.Films;

public class ToggleWatchListCommandHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    : IRequestHandler<ToggleWatchListCommand>
{
    public async Task Handle(ToggleWatchListCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя по его идентификатору 
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();

        // Проверяем, находится ли фильм в списке просмотра пользователя 
        if (user.Watchlist.Any(x => x.FilmId == request.Id))
            user.RemoveFilmFromWatchlist(request.Id); // Если да, удаляем фильм из списка просмотра 
        else
            user.AddFilmToWatchlist(await GetFilmAsync(request.Id)); // Если нет, добавляем фильм в список просмотра 

        // Обновляем данные пользователя в репозитории 
        await unitOfWork.UserRepository.Value.UpdateAsync(user);
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