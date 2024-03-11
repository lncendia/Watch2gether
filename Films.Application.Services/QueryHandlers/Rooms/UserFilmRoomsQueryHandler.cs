using Films.Application.Abstractions.DTOs.Rooms;
using Films.Application.Abstractions.Queries.Rooms;
using Films.Application.Services.Mappers.Rooms;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Films.Specifications;
using Films.Domain.Rooms.FilmRooms.Specifications;
using Films.Domain.Servers.Specifications;
using MediatR;

namespace Films.Application.Services.QueryHandlers.Rooms;

public class UserFilmRoomsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<UserFilmRoomsQuery, IReadOnlyCollection<FilmRoomDto>>
{
    public async Task<IReadOnlyCollection<FilmRoomDto>> Handle(UserFilmRoomsQuery request, CancellationToken cancellationToken)
    {
        var roomsSpecification = new FilmRoomByUserSpecification(request.Id);
        var rooms = await unitOfWork.FilmRoomRepository.Value.FindAsync(roomsSpecification);

        var filmsSpecification = new FilmsByIdsSpecification(rooms.Select(x => x.FilmId));
        var films = await unitOfWork.FilmRepository.Value.FindAsync(filmsSpecification);


        var serversSpecification = new ServersByIdsSpecification(rooms.Select(x => x.ServerId));
        var servers = await unitOfWork.ServerRepository.Value.FindAsync(serversSpecification);

        List<FilmRoomDto> filmRooms = [];

        foreach (var room in rooms)
        {
            var film = films.First(f => f.Id == room.FilmId);
            var server = servers.First(s => s.Id == room.ServerId);
            filmRooms.Add(Mapper.Map(room, film, server));
        }

        return filmRooms;
    }
}