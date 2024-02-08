using MediatR;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды на изменение номера сезона и серии
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class ChangeSeriesCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeSeriesCommand>
{
    public async Task Handle(ChangeSeriesCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату из репозитория
        var room = await unitOfWork.FilmRoomRepository.Value.GetAsync(request.RoomId);

        // Если комната не найдена - вызываем исключение
        if (room == null) throw new RoomNotFoundException();

        // Получаем фильм
        var film = await unitOfWork.FilmRepository.Value.GetAsync(room.FilmId);

        // Если фильм не найден - вызываем исключение
        if (film == null) throw new FilmNotFoundException();

        // Меняем сезон и эпизод
        room.ChangeSeries(film, request.UserId, request.Season, request.Series);
              
        // Обновляем комнату в репозитории
        await unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}