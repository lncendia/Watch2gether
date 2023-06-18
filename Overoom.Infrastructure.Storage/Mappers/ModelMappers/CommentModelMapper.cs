using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Comments.Entities;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Models.Comment;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class CommentModelMapper : IModelMapperUnit<CommentModel, Comment>
{
    private readonly ApplicationDbContext _context;

    public CommentModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<CommentModel> MapAsync(Comment entity)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == entity.Id) ??
                      new CommentModel { Id = entity.Id };
        comment.FilmId = entity.FilmId;
        comment.Text = entity.Text;
        comment.CreatedAt = entity.CreatedAt;
        comment.UserId = entity.UserId;
        return comment;
    }
}