using Overoom.Domain.Rooms.YoutubeRoom.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Rooms.YoutubeRoom.Specifications;

public class OpenYoutubeRoomsSpecification : ISpecification<YoutubeRoom, IYoutubeRoomSpecificationVisitor>
{
    public OpenYoutubeRoomsSpecification(bool isOpen) => IsOpen = isOpen;

    public bool IsOpen { get; }

    public bool IsSatisfiedBy(YoutubeRoom item) => item.IsOpen == IsOpen;

    public void Accept(IYoutubeRoomSpecificationVisitor visitor) => visitor.Visit(this);
}