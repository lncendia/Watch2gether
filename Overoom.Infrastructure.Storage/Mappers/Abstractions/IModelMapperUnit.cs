using Overoom.Domain.Abstractions;
using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Mappers.Abstractions;

internal interface IModelMapperUnit<TModel, in TAggregate>
    where TAggregate : AggregateRoot where TModel : IAggregateModel
{
    Task<TModel> MapAsync(TAggregate model);
}