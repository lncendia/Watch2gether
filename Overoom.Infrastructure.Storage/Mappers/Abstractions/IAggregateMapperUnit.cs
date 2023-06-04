using Overoom.Domain.Abstractions;
using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Mappers.Abstractions;

internal interface IAggregateMapperUnit<out TAggregate, in TModel>
    where TAggregate : AggregateRoot where TModel : IAggregateModel
{
    TAggregate Map(TModel model);
}