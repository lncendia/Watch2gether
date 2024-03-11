using Films.Application.Abstractions.Commands.Rooms.YoutubeRooms;
using Films.Application.Abstractions.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.Rooms.YoutubeRooms;

/// <summary>
/// Обработчик команды на блокировку пользователя в комнате ютуб
/// </summary>
public class BlockViewerCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<BlockViewerCommand>
{
    public async Task Handle(BlockViewerCommand request, CancellationToken cancellationToken)
    {
        var room = await unitOfWork.YoutubeRoomRepository.Value.GetAsync(request.RoomId);
        if (room == null) throw new RoomNotFoundException();

        room.Block(request.UserId);

        await unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        await unitOfWork.SaveChangesAsync();
    }
}