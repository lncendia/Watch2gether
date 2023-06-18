using Overoom.Domain.Abstractions.Interfaces;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;
using Overoom.Domain.Rooms.YoutubeRoom.Ordering.Visitor;
using Overoom.Domain.Rooms.YoutubeRoom.Specifications.Visitor;

namespace Overoom.Domain.Abstractions.Repositories;

public interface IYoutubeRoomRepository : IRepository<YoutubeRoom, Guid, IYoutubeRoomSpecificationVisitor, IYoutubeRoomSortingVisitor>
{
}