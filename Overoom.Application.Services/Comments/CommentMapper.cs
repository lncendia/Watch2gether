using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Comments.DTOs;
using Overoom.Application.Abstractions.Comments.Interfaces;
using Overoom.Domain.Comments.Entities;
using Overoom.Domain.Users.Entities;

namespace Overoom.Application.Services.Comments;

public class CommentMapper : ICommentMapper
{
    public CommentDto Map(Comment comment, User user)
    {
        return new CommentDto(comment.Id, user.Id, comment.Text, comment.CreatedAt, user.Name, user.AvatarUri);
    }
}