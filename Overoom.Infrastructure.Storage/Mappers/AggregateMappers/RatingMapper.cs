using Overoom.Domain.Rating;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.Rating;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class RatingMapper : IAggregateMapperUnit<Rating, RatingModel>
{
    public Rating Map(RatingModel model)
    {
        var rating = new Rating(model.FilmId, model.UserId, model.Score);
        IdFields.AggregateId.SetValue(rating, model.Id);
        return rating;
    }
}