using System.Linq.Expressions;
using Watch2gether.Domain.Rooms;
using Watch2gether.Domain.Rooms.BaseRoom;
using Watch2gether.Domain.Rooms.YoutubeRoom;
using Watch2gether.Domain.Rooms.YoutubeRoom.Specifications;
using Watch2gether.Domain.Rooms.YoutubeRoom.Specifications.Visitor;
using Watch2gether.Domain.Specifications;
using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Infrastructure.PersistentStorage.Models;
using Watch2gether.Infrastructure.PersistentStorage.Models.Rooms;

namespace Watch2gether.Infrastructure.PersistentStorage.Visitors.Specifications;

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