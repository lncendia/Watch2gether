using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Rooms.YoutubeRoom.Specifications.Visitor;

public interface
    IYoutubeRoomSpecificationVisitor : ISpecificationVisitor<IYoutubeRoomSpecificationVisitor, Entities.YoutubeRoom>
{
    void Visit(OpenYoutubeRoomsSpecification specification);
}