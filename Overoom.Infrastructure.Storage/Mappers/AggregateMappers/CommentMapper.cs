using System.Reflection;
using Overoom.Domain.Comment.Entities;
using Overoom.Domain.Film.Entities;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.Comments;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class CommentMapper : IAggregateMapperUnit<Comment, CommentModel>
{
    private static readonly FieldInfo CreatedAt =
        typeof(Film).GetField("<CreatedAt>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public Comment Map(CommentModel model)
    {
        var comment = new Comment(model.FilmId, model.UserId, model.Text);
        IdFields.AggregateId.SetValue(comment, model.Id);
        CreatedAt.SetValue(comment, model.CreatedAt);
        return comment;
    }
}