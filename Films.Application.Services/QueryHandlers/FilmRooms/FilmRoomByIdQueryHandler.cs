using Films.Application.Abstractions.DTOs.Rooms;
using Films.Application.Abstractions.Exceptions;
using Films.Application.Abstractions.Queries.FilmRooms;
using Films.Application.Services.Common;
using Films.Application.Services.Mappers.Rooms;
using Films.Domain.Abstractions.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Films.Application.Services.QueryHandlers.FilmRooms;

public class FilmRoomByIdQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<FilmRoomByIdQuery, FilmRoomDto>
{
    public async Task<FilmRoomDto> Handle(FilmRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await unitOfWork.FilmRoomRepository.Value.GetAsync(request.Id);
        if (room == null) throw new RoomNotFoundException();

        var film = await cache.TryGetFilmFromCacheAsync(room.FilmId, unitOfWork);

        var server = await unitOfWork.ServerRepository.Value.GetAsync(room.ServerId);
        if (server == null) throw new ServerNotFoundException();
        
        return Mapper.Map(room, film, server);
    }
}