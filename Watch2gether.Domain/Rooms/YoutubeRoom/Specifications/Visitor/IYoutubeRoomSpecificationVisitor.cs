using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Rooms.YoutubeRoom.Specifications.Visitor;

public interface
    IYoutubeRoomSpecificationVisitor : ISpecificationVisitor<IYoutubeRoomSpecificationVisitor, YoutubeRoom>
{
    void Visit(OpenYoutubeRoomsSpecification specification);
}