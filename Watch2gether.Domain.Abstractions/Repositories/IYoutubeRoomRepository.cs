using Watch2gether.Domain.Abstractions.Interfaces;
using Watch2gether.Domain.Rooms.YoutubeRoom;
using Watch2gether.Domain.Rooms.YoutubeRoom.Ordering.Visitor;
using Watch2gether.Domain.Rooms.YoutubeRoom.Specifications.Visitor;

namespace Watch2gether.Domain.Abstractions.Repositories;

public interface IYoutubeRoomRepository : IRepository<YoutubeRoom, Guid, IYoutubeRoomSpecificationVisitor, IYoutubeRoomSortingVisitor>
{
}