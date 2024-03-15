using Room.Domain.Specifications.Abstractions;

namespace Room.Domain.Messages.YoutubeMessages.Specifications.Visitor;

public interface
    IYoutubeMessageSpecificationVisitor : ISpecificationVisitor<IYoutubeMessageSpecificationVisitor, YoutubeMessage>
{
    void Visit(MessagesFromDateSpecification spec);
    void Visit(RoomMessagesSpecification spec);
}