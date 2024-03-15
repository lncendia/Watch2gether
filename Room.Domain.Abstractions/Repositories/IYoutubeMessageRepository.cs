using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Messages.YoutubeMessages;
using Room.Domain.Messages.YoutubeMessages.Ordering.Visitor;
using Room.Domain.Messages.YoutubeMessages.Specifications.Visitor;

namespace Room.Domain.Abstractions.Repositories;

public interface IYoutubeMessageRepository : IRepository<YoutubeMessage, Guid, IYoutubeMessageSpecificationVisitor, IYoutubeMessageSortingVisitor>;