using Room.Domain.Messages.YoutubeMessages.Specifications.Visitor;
using Room.Domain.Specifications.Abstractions;

namespace Room.Domain.Messages.YoutubeMessages.Specifications;

public class MessagesFromDateSpecification(DateTime maxTime)
    : ISpecification<YoutubeMessage, IYoutubeMessageSpecificationVisitor>
{
    public DateTime MaxTime { get; } = maxTime;

    public bool IsSatisfiedBy(YoutubeMessage item) => item.CreatedAt < MaxTime;

    public void Accept(IYoutubeMessageSpecificationVisitor visitor) => visitor.Visit(this);
}