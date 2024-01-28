using Films.Domain.Abstractions;
using Films.Infrastructure.Storage.Models.Abstractions;

namespace Films.Infrastructure.Storage.Mappers.Abstractions;

public interface IModelMapperUnit<TModel, in TAggregate>
    where TAggregate : AggregateRoot where TModel : IAggregateModel
{
    Task<TModel> MapAsync(TAggregate model);
}