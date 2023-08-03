using Overoom.Application.Abstractions.Comments.DTOs;
using Overoom.Domain.Comments.Entities;
using Overoom.Domain.Users.Entities;

namespace Overoom.Application.Abstractions.Comments.Interfaces;

public interface ICommentMapper
{
    public CommentDto Map(Comment comment, User user);
}