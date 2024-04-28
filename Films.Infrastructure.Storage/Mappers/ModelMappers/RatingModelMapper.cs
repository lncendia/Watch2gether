using Films.Domain.Ratings;
using Microsoft.EntityFrameworkCore;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Ratings;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

internal class RatingModelMapper(ApplicationDbContext context) : IModelMapperUnit<RatingModel, Rating>
{
    public async Task<RatingModel> MapAsync(Rating aggregate)
    {
        var model = await context.Ratings.FirstOrDefaultAsync(x => x.Id == aggregate.Id) ??
                    new RatingModel { Id = aggregate.Id };

        model.FilmId = aggregate.FilmId;
        model.Score = aggregate.Score;
        model.UserId = aggregate.UserId;
        model.Date = aggregate.Date;

        return model;
    }
}