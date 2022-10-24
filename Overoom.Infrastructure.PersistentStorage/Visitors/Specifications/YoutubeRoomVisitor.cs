using System.Linq.Expressions;
using Overoom.Infrastructure.PersistentStorage.Models.Rooms;
using Overoom.Domain.Rooms;
using Overoom.Domain.Rooms.BaseRoom;
using Overoom.Domain.Rooms.YoutubeRoom;
using Overoom.Domain.Rooms.YoutubeRoom.Specifications;
using Overoom.Domain.Rooms.YoutubeRoom.Specifications.Visitor;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Infrastructure.PersistentStorage.Models;

namespace Overoom.Infrastructure.PersistentStorage.Visitors.Specifications;

public class YoutubeRoomVisitor : BaseVisitor<YoutubeRoomModel, IYoutubeRoomSpecificationVisitor, YoutubeRoom>,
    IYoutubeRoomSpecificationVisitor
{
    protected override Expression<Func<YoutubeRoomModel, bool>> ConvertSpecToExpression(
        ISpecification<YoutubeRoom, IYoutubeRoomSpecificationVisitor> spec)
    {
        var visitor = new YoutubeRoomVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }

    public void Visit(OpenYoutubeRoomsSpecification specification) => Expr = x => x.IsOpen == specification.IsOpen;
}