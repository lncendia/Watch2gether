using Films.Application.Abstractions.Commands.FilmsManagement;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Films.Entities;

namespace Films.Application.Services.Commands.FilmsManagement;

public class DeleteFilmCommandHandler(IUnitOfWork unitOfWork, IPosterService posterService, IMemoryCache memoryCache)
    : IRequestHandler<DeleteFilmCommand>
{
    public async Task Handle(DeleteFilmCommand request, CancellationToken cancellationToken)
    {
        var film = await GetFilmAsync(request.Id);
        await posterService.DeleteAsync(film.PosterUrl);
        await unitOfWork.FilmRepository.Value.DeleteAsync(request.Id);
        await unitOfWork.SaveChangesAsync();
        memoryCache.Remove(request.Id);
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