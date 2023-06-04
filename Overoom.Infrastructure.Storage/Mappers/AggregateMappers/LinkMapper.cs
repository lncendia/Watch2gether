using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class LinkMapper : IAggregateMapperUnit<Link, LinkModel>
{
    private static readonly List<Link> MockList = new();
    public Link Map(LinkModel model)
    {
        var link = new Link(model.User1Id, MockList, model.User2Id);
        IdFields.AggregateId.SetValue(link, model.Id);
        if (model.IsAccepted) link.Confirm(MockList);
        return link;
    }
}