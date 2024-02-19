using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Queries.Rooms;
using Films.Application.Abstractions.Queries.Rooms.DTOs;
using Films.Application.Services.Common;
using Films.Domain.Abstractions.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Films.Application.Services.QueryHandlers.Rooms;

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