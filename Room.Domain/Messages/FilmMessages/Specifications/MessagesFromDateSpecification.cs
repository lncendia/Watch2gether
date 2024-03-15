using Room.Domain.Messages.FilmMessages.Specifications.Visitor;
using Room.Domain.Specifications.Abstractions;

namespace Room.Domain.Messages.FilmMessages.Specifications;

public class MessagesFromDateSpecification(DateTime maxTime)
    : ISpecification<FilmMessage, IFilmMessageSpecificationVisitor>
{
    public DateTime MaxTime { get; } = maxTime;

    public bool IsSatisfiedBy(FilmMessage item) => item.CreatedAt < MaxTime;

    public void Accept(IFilmMessageSpecificationVisitor visitor) => visitor.Visit(this);
}