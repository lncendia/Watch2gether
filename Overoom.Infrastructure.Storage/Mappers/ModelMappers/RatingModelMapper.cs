using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Ratings;
using Overoom.Domain.Ratings.Entities;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Models.Rating;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class RatingModelMapper : IModelMapperUnit<RatingModel, Rating>
{
    private readonly ApplicationDbContext _context;

    public RatingModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<RatingModel> MapAsync(Rating entity)
    {
        var rating = _context.Ratings.Local.FirstOrDefault(x => x.Id == entity.Id);
        if (rating == null)
        {
            rating = await _context.Ratings.FirstOrDefaultAsync(x => x.Id == entity.Id) ??
                     new RatingModel { Id = entity.Id };
        }

        rating.FilmId = entity.FilmId;
        rating.Score = entity.Score;
        rating.UserId = entity.UserId;
        rating.Date = entity.Date;
        return rating;
    }
}