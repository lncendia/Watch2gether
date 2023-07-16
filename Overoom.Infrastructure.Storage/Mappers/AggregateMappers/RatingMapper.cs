using System.Reflection;
using Overoom.Domain.Abstractions;
using Overoom.Domain.Ratings;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.Rating;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class RatingMapper : IAggregateMapperUnit<Rating, RatingModel>
{
    private static readonly FieldInfo UserId =
        typeof(Rating).GetField("<UserId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo Date =
        typeof(Rating).GetField("<Date>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public Rating Map(RatingModel model)
    {
        var rating = new Rating(model.FilmId, Guid.Empty, model.Score);
        IdFields.AggregateId.SetValue(rating, model.Id);
        UserId.SetValue(rating, model.UserId);
        Date.SetValue(rating, model.Date);
        var domainCollection = (List<IDomainEvent>)IdFields.DomainEvents.GetValue(rating)!;
        domainCollection.Clear();
        return rating;
    }
}