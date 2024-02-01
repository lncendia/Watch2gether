using Films.Application.Abstractions.Commands.FilmsManagement;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Films.Entities;

namespace Films.Application.Services.Commands.FilmsManagement;

public class ChangeFilmCommandHandler(IUnitOfWork unitOfWork, IPosterService posterService, IMemoryCache memoryCache)
    : IRequestHandler<ChangeFilmCommand>
{
    public async Task Handle(ChangeFilmCommand request, CancellationToken cancellationToken)
    {
        var film = await GetFilmAsync(request.Id);
        if (!string.IsNullOrEmpty(request.Description)) film.Description = request.Description;
        if (!string.IsNullOrEmpty(request.ShortDescription)) film.ShortDescription = request.ShortDescription;
        if (request.RatingKp.HasValue) film.RatingKp = request.RatingKp.Value;
        if (request.RatingImdb.HasValue) film.RatingImdb = request.RatingImdb.Value;
        
        if (request is { CountSeasons: not null, CountEpisodes: not null })
            film.UpdateSeriesInfo(request.CountSeasons.Value, request.CountEpisodes.Value);

        Uri? poster = null;
        if (request.PosterUrl != null) poster = await posterService.SaveAsync(request.PosterUrl);
        else if (request.PosterBase64 != null) poster = await posterService.SaveAsync(request.PosterBase64);

        if (poster != null)
        {
            await posterService.DeleteAsync(film.PosterUrl);
            film.PosterUrl = poster;
        }

        if (request.CdnList != null)
        {
            foreach (var cdn in request.CdnList) film.AddOrChangeCdn(cdn);
        }

        await unitOfWork.FilmRepository.Value.UpdateAsync(film);
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