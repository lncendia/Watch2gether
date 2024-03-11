using System.Reflection;
using System.Runtime.CompilerServices;
using Films.Domain.Abstractions;
using Films.Domain.Ratings;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Mappers.StaticMethods;
using Films.Infrastructure.Storage.Models.Rating;

namespace Films.Infrastructure.Storage.Mappers.AggregateMappers;

internal class RatingMapper : IAggregateMapperUnit<Rating, RatingModel>
{
    private static readonly Type RatingType = typeof(Rating);

    private static readonly FieldInfo UserId =
        RatingType.GetField("<UserId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo Date =
        RatingType.GetField("<Date>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo FilmId =
        RatingType.GetField("<FilmId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo Score =
        RatingType.GetField("<Score>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;


    public Rating Map(RatingModel model)
    {
        var rating = (Rating)RuntimeHelpers.GetUninitializedObject(RatingType);
        IdFields.AggregateId.SetValue(rating, model.Id);
        UserId.SetValue(rating, model.UserId);
        Date.SetValue(rating, model.Date);
        FilmId.SetValue(rating, model.FilmId);
        Score.SetValue(rating, model.Score);
        IdFields.DomainEvents.SetValue(rating, new List<IDomainEvent>());
        return rating;
    }
}