using Room.Domain.Specifications.Abstractions;

namespace Room.Domain.Messages.FilmMessages.Specifications.Visitor;

public interface IFilmMessageSpecificationVisitor : ISpecificationVisitor<IFilmMessageSpecificationVisitor, FilmMessage>
{
    void Visit(MessagesFromDateSpecification spec);
    void Visit(RoomMessagesSpecification spec);
}