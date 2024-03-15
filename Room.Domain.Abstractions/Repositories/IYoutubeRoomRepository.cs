using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Rooms.YoutubeRooms;
using Room.Domain.Rooms.YoutubeRooms.Ordering.Visitor;
using Room.Domain.Rooms.YoutubeRooms.Specifications.Visitor;

namespace Room.Domain.Abstractions.Repositories;

public interface IYoutubeRoomRepository : IRepository<YoutubeRoom, Guid, IYoutubeRoomSpecificationVisitor, IYoutubeRoomSortingVisitor>;