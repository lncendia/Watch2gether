using Room.Domain.Abstractions;
using Room.Infrastructure.Storage.Models.Abstractions;

namespace Room.Infrastructure.Storage.Mappers.Abstractions;

public interface IModelMapperUnit<TModel, in TAggregate>
    where TAggregate : AggregateRoot where TModel : IAggregateModel
{
    Task<TModel> MapAsync(TAggregate model);
}