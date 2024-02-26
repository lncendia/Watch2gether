using Films.Application.Abstractions.Commands.Rooms.DTOs;
using Films.Application.Abstractions.Commands.Rooms.YoutubeRooms;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.Rooms.YoutubeRooms;

/// <summary>
/// Обработчик команды на подключение к комнате ютуб
/// </summary>
public class ConnectRoomCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ConnectRoomCommand, RoomServerDto>
{
    public async Task<RoomServerDto> Handle(ConnectRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await unitOfWork.YoutubeRoomRepository.Value.GetAsync(request.RoomId);
        if (room == null) throw new RoomNotFoundException();

        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();

        var server = await unitOfWork.ServerRepository.Value.GetAsync(room.ServerId);
        if (server == null) throw new ServerNotFoundException();

        room.Connect(user, request.Code);

        await unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        await unitOfWork.SaveChangesAsync();

        return new RoomServerDto
        {
            Id = room.Id,
            ServerUrl = server.Url
        };
    }
}