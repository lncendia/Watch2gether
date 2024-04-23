using Films.Domain.Abstractions;
using Films.Infrastructure.Storage.Models.Abstractions;

namespace Films.Infrastructure.Storage.Mappers.Abstractions;

public interface IAggregateMapperUnit<out TAggregate, in TModel>
    where TAggregate : AggregateRoot where TModel : IAggregateModel
{
    TAggregate Map(TModel model);
}