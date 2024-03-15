using Films.Domain.Comments;
using Microsoft.EntityFrameworkCore;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Comments;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

internal class CommentModelMapper(ApplicationDbContext context) : IModelMapperUnit<CommentModel, Comment>
{
    public async Task<CommentModel> MapAsync(Comment aggregate)
    {
        var model = await context.Comments.FirstOrDefaultAsync(x => x.Id == aggregate.Id) ??
                      new CommentModel { Id = aggregate.Id };
        model.FilmId = aggregate.FilmId;
        model.Text = aggregate.Text;
        model.CreatedAt = aggregate.CreatedAt;
        model.UserId = aggregate.UserId;
        return model;
    }
}