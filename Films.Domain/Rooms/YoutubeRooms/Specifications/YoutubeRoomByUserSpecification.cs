using Films.Domain.Rooms.YoutubeRooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Rooms.YoutubeRooms.Specifications;

public class YoutubeRoomByUserSpecification(Guid userId) : ISpecification<YoutubeRoom, IYoutubeRoomSpecificationVisitor>
{
    public Guid UserId { get;  } = userId;

    public void Accept(IYoutubeRoomSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(YoutubeRoom item) => item.Viewers.Any(u => u == UserId);
}