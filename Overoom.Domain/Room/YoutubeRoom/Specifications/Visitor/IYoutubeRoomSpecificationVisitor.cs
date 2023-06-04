using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Room.YoutubeRoom.Specifications.Visitor;

public interface
    IYoutubeRoomSpecificationVisitor : ISpecificationVisitor<IYoutubeRoomSpecificationVisitor, Entities.YoutubeRoom>
{
    void Visit(OpenYoutubeRoomsSpecification specification);
}