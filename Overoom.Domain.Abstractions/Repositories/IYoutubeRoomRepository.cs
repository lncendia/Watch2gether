using Overoom.Domain.Abstractions.Interfaces;
using Overoom.Domain.Room.YoutubeRoom.Entities;
using Overoom.Domain.Room.YoutubeRoom.Ordering.Visitor;
using Overoom.Domain.Room.YoutubeRoom.Specifications.Visitor;

namespace Overoom.Domain.Abstractions.Repositories;

public interface IYoutubeRoomRepository : IRepository<YoutubeRoom, Guid, IYoutubeRoomSpecificationVisitor, IYoutubeRoomSortingVisitor>
{
}