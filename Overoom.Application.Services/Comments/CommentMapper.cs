using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Comments.DTOs;
using Overoom.Application.Abstractions.Comments.Interfaces;
using Overoom.Domain.Comments.Entities;
using Overoom.Domain.Users.Entities;

namespace Overoom.Application.Services.Comments;

public class CommentMapper : ICommentMapper
{
    public CommentDto Map(Comment comment, User? user)
    {
        string? name;
        Uri? avatar;
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