using MediatR;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Abstractions.Common.Exceptions;
using Room.Application.Abstractions.DTOs.FilmRoom;
using Room.Application.Services.Mappers;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Rooms.FilmRoom.Entities;

namespace Room.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды создания комнаты
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
public class CreateRoomCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateRoomCommand, FilmRoomDto>
{
    public async Task<FilmRoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        // Получаем фильм
        var film = await unitOfWork.FilmRepository.Value.GetAsync(request.FilmId);

        // Если фильм не найден - вызываем исключение
        if (film == null) throw new FilmNotFoundException();

        // Получаем пользователя
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);

        // Если пользователь не найден - вызываем исключение
        if (user == null) throw new UserNotFoundException();

        // Создаем комнату
        var room = new FilmRoom(user, film, request.CdnName, request.IsOpen);

        // Добавляем комнату в репозиторий
        await unitOfWork.FilmRoomRepository.Value.AddAsync(room);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
        
        // Преобразовываем комнату в DTO и возвращаем
        return await FilmRoomMapper.MapAsync(room, unitOfWork);
    }
}