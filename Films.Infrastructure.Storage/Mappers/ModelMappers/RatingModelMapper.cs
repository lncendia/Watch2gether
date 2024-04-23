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
        return await context.Ratings.FirstOrDefaultAsync(x => x.Id == aggregate.Id) ?? new RatingModel
        {
            Id = aggregate.Id,
            FilmId = aggregate.FilmId,
            Score = aggregate.Score,
            UserId = aggregate.UserId,
            Date = aggregate.Date
        };
    }
}