using System.Linq.Expressions;
using Overoom.Domain.Room.YoutubeRoom.Entities;
using Overoom.Domain.Room.YoutubeRoom.Specifications;
using Overoom.Domain.Room.YoutubeRoom.Specifications.Visitor;
using Overoom.Domain.Rooms;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Infrastructure.Storage.Models;
using Overoom.Infrastructure.Storage.Models.Rooms;

namespace Overoom.Infrastructure.Storage.Visitors.Specifications;

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