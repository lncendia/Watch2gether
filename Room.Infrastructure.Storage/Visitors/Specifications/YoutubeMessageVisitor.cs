using System.Linq.Expressions;
using Room.Domain.Messages.YoutubeMessages;
using Room.Domain.Messages.YoutubeMessages.Specifications;
using Room.Domain.Messages.YoutubeMessages.Specifications.Visitor;
using Room.Domain.Specifications.Abstractions;
using Room.Infrastructure.Storage.Models.YoutubeRooms;
using Room.Infrastructure.Storage.Models.Messages;

namespace Room.Infrastructure.Storage.Visitors.Specifications;

public class YoutubeMessageVisitor :
    BaseVisitor<MessageModel<YoutubeRoomModel>, IYoutubeMessageSpecificationVisitor, YoutubeMessage>,
    IYoutubeMessageSpecificationVisitor
{
    protected override Expression<Func<MessageModel<YoutubeRoomModel>, bool>> ConvertSpecToExpression(
        ISpecification<YoutubeMessage, IYoutubeMessageSpecificationVisitor> spec)
    {
        var visitor = new YoutubeMessageVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }

    public void Visit(MessagesFromDateSpecification spec) => Expr = model => model.CreatedAt < spec.MaxTime;

    public void Visit(RoomMessagesSpecification spec) => Expr = model => model.RoomId == spec.RoomId;
}