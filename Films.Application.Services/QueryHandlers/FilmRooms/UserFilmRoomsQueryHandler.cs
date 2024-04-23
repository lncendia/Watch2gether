using Films.Application.Abstractions.DTOs.Rooms;
using Films.Application.Abstractions.Queries.FilmRooms;
using Films.Application.Services.Mappers.Rooms;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Films.Specifications;
using Films.Domain.Ordering;
using Films.Domain.Rooms.FilmRooms;
using Films.Domain.Rooms.FilmRooms.Ordering;
using Films.Domain.Rooms.FilmRooms.Ordering.Visitor;
using Films.Domain.Rooms.FilmRooms.Specifications;
using MediatR;

namespace Films.Application.Services.QueryHandlers.FilmRooms;

public class UserFilmRoomsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<UserFilmRoomsQuery, IReadOnlyCollection<FilmRoomShortDto>>
{
    public async Task<IReadOnlyCollection<FilmRoomShortDto>> Handle(UserFilmRoomsQuery request, CancellationToken cancellationToken)
    {
        
        var order = new DescendingOrder<FilmRoom, IFilmRoomSortingVisitor>(new FilmRoomOrderByDate());

        var rooms = await unitOfWork.FilmRoomRepository.Value.FindAsync(new FilmRoomByUserSpecification(request.UserId), order);
        
        var filmsSpecification = new FilmsByIdsSpecification(rooms.Select(x => x.FilmId));
        var films = await unitOfWork.FilmRepository.Value.FindAsync(filmsSpecification);

        List<FilmRoomShortDto> filmRooms = [];

        foreach (var room in rooms)
        {
            var film = films.First(f => f.Id == room.FilmId);
            filmRooms.Add(Mapper.Map(room, film));
        }

        return filmRooms;
    }
}