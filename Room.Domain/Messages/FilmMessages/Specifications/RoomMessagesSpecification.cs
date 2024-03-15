using Room.Domain.Messages.FilmMessages.Specifications.Visitor;
using Room.Domain.Specifications.Abstractions;

namespace Room.Domain.Messages.FilmMessages.Specifications;

public class RoomMessagesSpecification(Guid roomId)
    : ISpecification<FilmMessage, IFilmMessageSpecificationVisitor>
{
    public Guid RoomId { get; } = roomId;

    public bool IsSatisfiedBy(FilmMessage item) => item.RoomId == RoomId;

    public void Accept(IFilmMessageSpecificationVisitor visitor) => visitor.Visit(this);
}