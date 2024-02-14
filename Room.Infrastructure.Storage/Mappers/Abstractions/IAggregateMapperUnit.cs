using Room.Domain.Abstractions;
using Room.Infrastructure.Storage.Models.Abstractions;

namespace Room.Infrastructure.Storage.Mappers.Abstractions;

public interface IAggregateMapperUnit<out TAggregate, in TModel>
    where TAggregate : AggregateRoot where TModel : IAggregateModel
{
    TAggregate Map(TModel model);
}