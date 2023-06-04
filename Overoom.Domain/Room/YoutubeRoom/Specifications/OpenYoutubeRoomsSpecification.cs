using Overoom.Domain.Room.YoutubeRoom.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Room.YoutubeRoom.Specifications;

public class OpenYoutubeRoomsSpecification : ISpecification<Entities.YoutubeRoom, IYoutubeRoomSpecificationVisitor>
{
    public OpenYoutubeRoomsSpecification(bool isOpen) => IsOpen = isOpen;

    public bool IsOpen { get; }

    public bool IsSatisfiedBy(Entities.YoutubeRoom item) => item.IsOpen == IsOpen;

    public void Accept(IYoutubeRoomSpecificationVisitor visitor) => visitor.Visit(this);
}