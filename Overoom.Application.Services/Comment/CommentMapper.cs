using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Comment.DTOs;
using Overoom.Application.Abstractions.Comment.Interfaces;

namespace Overoom.Application.Services.Comment;

public class CommentMapper : ICommentMapper
{
    public CommentDto Map(Domain.Comment.Entities.Comment comment, Domain.User.Entities.User? user)
    {
        string? name;
        string? avatar;
        if (comment.UserId == null || user == null)
        {
            name = "Удаленный пользователь";
            avatar = ApplicationConstants.DefaultAvatar;
        }
        else
        {
            name = user.Name;
            avatar = user.AvatarUri;
        }

        return new CommentDto(comment.Id, comment.Text, comment.CreatedAt, name, avatar);
    }
}