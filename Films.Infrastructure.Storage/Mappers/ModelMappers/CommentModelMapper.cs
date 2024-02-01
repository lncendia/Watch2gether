using Microsoft.EntityFrameworkCore;
using Films.Domain.Comments.Entities;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Comment;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

internal class CommentModelMapper(ApplicationDbContext context) : IModelMapperUnit<CommentModel, Comment>
{
    public async Task<CommentModel> MapAsync(Comment entity)
    {
        var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == entity.Id) ??
                      new CommentModel { Id = entity.Id };
        comment.FilmId = entity.FilmId;
        comment.Text = entity.Text;
        comment.CreatedAt = entity.CreatedAt;
        comment.UserId = entity.UserId;
        return comment;
    }
}