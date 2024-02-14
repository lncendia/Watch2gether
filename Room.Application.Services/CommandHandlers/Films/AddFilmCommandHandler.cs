using MediatR;
using Room.Application.Abstractions.Commands.Films;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Films.Entities;

namespace Room.Application.Services.CommandHandlers.Films;

/// <summary>
/// Обработчик команды на добавление фильма
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class AddFilmCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddFilmCommand>
{
    public async Task Handle(AddFilmCommand request, CancellationToken cancellationToken)
    {
        // Создаем фильм
        var film = new Film(request.Id)
        {
            Title = request.Title,
            Description = request.Description,
            CdnList = request.CdnList,
            PosterUrl = request.PosterUrl,
            Type = request.Type,
            Year = request.Year
        };

        // Добавляем в репозиторий
        await unitOfWork.FilmRepository.Value.AddAsync(film);

        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}