using Room.Domain.Messages.YoutubeMessages.Specifications.Visitor;
using Room.Domain.Specifications.Abstractions;

namespace Room.Domain.Messages.YoutubeMessages.Specifications;

public class RoomMessagesSpecification(Guid roomId)
    : ISpecification<YoutubeMessage, IYoutubeMessageSpecificationVisitor>
{
    public Guid RoomId { get; } = roomId;

    public bool IsSatisfiedBy(YoutubeMessage item) => item.RoomId == RoomId;

    public void Accept(IYoutubeMessageSpecificationVisitor visitor) => visitor.Visit(this);
}