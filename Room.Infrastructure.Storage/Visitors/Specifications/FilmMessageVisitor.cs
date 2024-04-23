using System.Linq.Expressions;
using Room.Domain.Messages.FilmMessages;
using Room.Domain.Messages.FilmMessages.Specifications;
using Room.Domain.Messages.FilmMessages.Specifications.Visitor;
using Room.Domain.Specifications.Abstractions;
using Room.Infrastructure.Storage.Models.FilmRooms;
using Room.Infrastructure.Storage.Models.Messages;

namespace Room.Infrastructure.Storage.Visitors.Specifications;

public class FilmMessageVisitor :
    BaseVisitor<MessageModel<FilmRoomModel>, IFilmMessageSpecificationVisitor, FilmMessage>,
    IFilmMessageSpecificationVisitor
{
    protected override Expression<Func<MessageModel<FilmRoomModel>, bool>> ConvertSpecToExpression(
        ISpecification<FilmMessage, IFilmMessageSpecificationVisitor> spec)
    {
        var visitor = new FilmMessageVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }

    public void Visit(MessagesFromDateSpecification spec) => Expr = model => model.CreatedAt < spec.MaxTime;

    public void Visit(RoomMessagesSpecification spec) => Expr = model => model.RoomId == spec.RoomId;
}