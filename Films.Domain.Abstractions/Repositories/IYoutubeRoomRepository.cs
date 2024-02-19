using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.YoutubeRooms;
using Films.Domain.Rooms.YoutubeRooms.Ordering.Visitor;
using Films.Domain.Rooms.YoutubeRooms.Specifications.Visitor;

namespace Films.Domain.Abstractions.Repositories;

public interface IYoutubeRoomRepository : IRepository<YoutubeRoom, Guid, IYoutubeRoomSpecificationVisitor, IYoutubeRoomSortingVisitor>;