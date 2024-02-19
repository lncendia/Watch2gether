using Films.Application.Abstractions.Commands.Rooms.FilmRooms;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.Rooms.FilmRooms;

/// <summary>
/// Обработчик команды на блокировку пользователя в комнате с фильмом
/// </summary>
public class BlockFilmViewerCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<BlockFilmViewerCommand>
{
    public async Task Handle(BlockFilmViewerCommand request, CancellationToken cancellationToken)
    {
        var room = await unitOfWork.FilmRoomRepository.Value.GetAsync(request.RoomId);
        if (room == null) throw new RoomNotFoundException();

        room.Block(request.UserId);

        await unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        await unitOfWork.SaveChangesAsync();
    }
}