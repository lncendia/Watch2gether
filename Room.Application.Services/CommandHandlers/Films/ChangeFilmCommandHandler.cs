using MediatR;
using Room.Application.Abstractions.Commands.Films;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.Films;

/// <summary>
/// Обработчик команды на изменение фильма
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class ChangeFilmCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeFilmCommand>
{
    public async Task Handle(ChangeFilmCommand request, CancellationToken cancellationToken)
    {
        // Получаем фильм
        var film = await unitOfWork.FilmRepository.Value.GetAsync(request.Id);

        // Если фильм не найден - вызываем исключение
        if (film == null) throw new FilmNotFoundException();

        // Устанавливаем описание
        film.Description = request.Description;

        // Устанавливаем URL постера
        film.PosterUrl = request.PosterUrl;

        // Добавляем или изменяем полученные CDN
        foreach (var cdn in film.CdnList) film.AddOrChangeCdn(cdn);

        // Обновляем фильм в репозитории
        await unitOfWork.FilmRepository.Value.UpdateAsync(film);

        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}