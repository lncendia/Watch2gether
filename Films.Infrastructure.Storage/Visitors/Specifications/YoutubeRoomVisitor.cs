using System.Linq.Expressions;
using Films.Domain.Rooms.YoutubeRooms;
using Films.Domain.Rooms.YoutubeRooms.Specifications;
using Films.Domain.Rooms.YoutubeRooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Models.Rooms.YoutubeRoom;

namespace Films.Infrastructure.Storage.Visitors.Specifications;

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

    public void Visit(YoutubeRoomByUserSpecification spec) => Expr = x => x.Viewers.Any(v => v.UserId == spec.UserId);

    public void Visit(OpenYoutubeRoomsSpecification specification) => Expr = x => x.Code != null;
}