using Films.Application.Abstractions.Commands.FilmRooms;
using Films.Application.Abstractions.DTOs.Rooms;
using Films.Application.Abstractions.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды на подключение к комнате с фильмом
/// </summary>
public class ConnectRoomCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ConnectRoomCommand, RoomServerDto>
{
    public async Task<RoomServerDto> Handle(ConnectRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await unitOfWork.FilmRoomRepository.Value.GetAsync(request.RoomId);
        if (room == null) throw new RoomNotFoundException();

        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();

        var server = await unitOfWork.ServerRepository.Value.GetAsync(room.ServerId);
        if (server == null) throw new ServerNotFoundException();

        room.Connect(user, request.Code);

        await unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        await unitOfWork.SaveChangesAsync();

        return new RoomServerDto
        {
            Id = room.Id,
            ServerUrl = server.Url
        };
    }
}