using Microsoft.EntityFrameworkCore;
using Films.Domain.Ratings;
using Films.Domain.Ratings.Entities;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Rating;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

internal class RatingModelMapper(ApplicationDbContext context) : IModelMapperUnit<RatingModel, Rating>
{
    public async Task<RatingModel> MapAsync(Rating entity)
    {
        var rating = context.Ratings.Local.FirstOrDefault(x => x.Id == entity.Id) ??
                     (await context.Ratings.FirstOrDefaultAsync(x => x.Id == entity.Id) ??
                      new RatingModel
                      {
                          Id = entity.Id,
                          FilmId = entity.FilmId,
                          Score = entity.Score,
                          UserId = entity.UserId,
                          Date = entity.Date
                      });

        return rating;
    }
}