using Films.Domain.Rooms.YoutubeRooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Rooms.YoutubeRooms.Specifications;

public class OpenYoutubeRoomsSpecification : ISpecification<YoutubeRoom, IYoutubeRoomSpecificationVisitor>
{
    public void Accept(IYoutubeRoomSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(YoutubeRoom item) => string.IsNullOrEmpty(item.Code);
}