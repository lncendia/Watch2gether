using Films.Application.Abstractions.DTOs.Rooms;
using Films.Application.Abstractions.Queries.FilmRooms;
using Films.Application.Services.Common;
using Films.Application.Services.Mappers.Rooms;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Films.Specifications;
using Films.Domain.Ordering;
using Films.Domain.Rooms.FilmRooms;
using Films.Domain.Rooms.FilmRooms.Ordering;
using Films.Domain.Rooms.FilmRooms.Ordering.Visitor;
using Films.Domain.Rooms.FilmRooms.Specifications;
using Films.Domain.Rooms.FilmRooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using MediatR;

namespace Films.Application.Services.QueryHandlers.FilmRooms;

public class SearchFilmRoomsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<SearchFilmRoomsQuery, (IReadOnlyCollection<FilmRoomDto> rooms, int count)>
{
    public async Task<(IReadOnlyCollection<FilmRoomDto> rooms, int count)> Handle(SearchFilmRoomsQuery request, CancellationToken cancellationToken)
    {
        ISpecification<FilmRoom, IFilmRoomSpecificationVisitor>? specification = null;

        if (request.FilmId.HasValue)
            specification.AddToSpecification(new FilmRoomByFilmSpecification(request.FilmId.Value));

        if (request is { UserId: not null, OnlyMy: true })
            specification.AddToSpecification(new FilmRoomByUserSpecification(request.UserId.Value));

        if (request.OnlyPublic)
            specification.AddToSpecification(new OpenFilmRoomsSpecification());


        var order = new DescendingOrder<FilmRoom, IFilmRoomSortingVisitor>(new FilmRoomOrderByDate());

        var rooms = await unitOfWork.FilmRoomRepository.Value.FindAsync(specification, order, request.Skip,
            request.Take);

        var count = await unitOfWork.FilmRoomRepository.Value.CountAsync(specification);
        
        var filmsSpecification = new FilmsByIdsSpecification(rooms.Select(x => x.FilmId));
        var films = await unitOfWork.FilmRepository.Value.FindAsync(filmsSpecification);

        List<FilmRoomDto> filmRooms = [];

        foreach (var room in rooms)
        {
            var film = films.First(f => f.Id == room.FilmId);
            filmRooms.Add(Mapper.Map(room, film, request.UserId));
        }

        return (filmRooms, count);
    }
}