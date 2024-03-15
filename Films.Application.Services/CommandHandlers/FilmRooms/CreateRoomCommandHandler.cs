using Films.Application.Abstractions.Commands.FilmRooms;
using Films.Application.Abstractions.DTOs.Rooms;
using Films.Application.Abstractions.Exceptions;
using Films.Application.Services.Common;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.FilmRooms;
using Films.Domain.Servers.Specifications;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Films.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды на создание комнаты с фильмом
/// </summary>
public class CreateRoomCommandHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    : IRequestHandler<CreateRoomCommand, RoomServerDto>
{
    public async Task<RoomServerDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var film = await memoryCache.TryGetFilmFromCacheAsync(request.FilmId, unitOfWork);

        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();
        
        var servers = await unitOfWork.ServerRepository.Value.FindAsync(new ServerByEnabledSpecification(true));

        var room = new FilmRoom(user, film, servers, request.IsOpen, request.CdnName);

        await unitOfWork.FilmRoomRepository.Value.AddAsync(room);
        await unitOfWork.SaveChangesAsync();

        return new RoomServerDto
        {
            Id = room.Id,
            ServerUrl = servers.First(s => s.Id == room.ServerId).Url,
            Code = room.Code
        };
    }
}