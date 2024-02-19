using Films.Application.Abstractions.Commands.Rooms.DTOs;
using Films.Application.Abstractions.Commands.Rooms.FilmRooms;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Services.Common;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.FilmRooms;
using Films.Domain.Servers.Specifications;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Films.Application.Services.CommandHandlers.Rooms.FilmRooms;

/// <summary>
/// Обработчик команды на создание комнаты с фильмом
/// </summary>
public class CreateFilmRoomCommandHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    : IRequestHandler<CreateFilmRoomCommand, RoomServerDto>
{
    public async Task<RoomServerDto> Handle(CreateFilmRoomCommand request, CancellationToken cancellationToken)
    {
        var film = await memoryCache.TryGetFilmFromCacheAsync(request.FilmId, unitOfWork);

        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();
        
        var servers = await unitOfWork.ServerRepository.Value.FindAsync(new ServerByEnabledSpecification(true));

        var room = new FilmRoom(user, film, servers, request.IsOpen, request.CdnName);

        await unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        await unitOfWork.SaveChangesAsync();

        return new RoomServerDto
        {
            Id = room.Id,
            ServerUrl = servers.First(s => s.Id == room.ServerId).Url,
            Code = room.Code
        };
    }
}