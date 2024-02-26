using MediatR;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.FilmRooms;
using Room.Domain.FilmRooms.Entities;

namespace Room.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды создания комнаты
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class CreateRoomCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateRoomCommand>
{
    public async Task Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        // Создаем комнату
        var room = new FilmRoom
        {
            Title = request.Title,
            CdnUrl = request.CdnUrl,
            IsSerial = request.IsSerial,
            Id = request.Id,
            Owner = new FilmViewer
            {
                Id = request.Owner.Id,
                Allows = request.Owner.Allows,
                PhotoUrl = request.Owner.PhotoUrl,
                Nickname = request.Owner.Nickname
            }
        };

        // Добавляем комнату в репозиторий
        await unitOfWork.FilmRoomRepository.Value.AddAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}