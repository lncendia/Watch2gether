using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Rooms.YoutubeRooms.Specifications.Visitor;

public interface IYoutubeRoomSpecificationVisitor : ISpecificationVisitor<IYoutubeRoomSpecificationVisitor, YoutubeRoom>
{
    void Visit(YoutubeRoomByUserSpecification spec);
    void Visit(OpenYoutubeRoomsSpecification spec);
}