using System.Reflection;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Ratings;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.Rating;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class RatingMapper : IAggregateMapperUnit<Rating, RatingModel>
{
    private static readonly FieldInfo UserId =
        typeof(Film).GetField("<UserId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public Rating Map(RatingModel model)
    {
        var rating = new Rating(model.FilmId, Guid.Empty, model.Score);
        IdFields.AggregateId.SetValue(rating, model.Id);
        UserId.SetValue(rating, model.UserId);
        return rating;
    }
}