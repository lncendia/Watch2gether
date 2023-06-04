using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class ProxyMapper : IAggregateMapperUnit<Proxy, ProxyModel>
{
    public Proxy Map(ProxyModel model)
    {
        var proxy = new Proxy(model.Host, model.Port, model.Login, model.Password);
        IdFields.AggregateId.SetValue(proxy, model.Id);
        return proxy;
    }
}