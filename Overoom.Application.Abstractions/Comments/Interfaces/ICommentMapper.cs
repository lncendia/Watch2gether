using Overoom.Application.Abstractions.Comments.DTOs;

namespace Overoom.Application.Abstractions.Comments.Interfaces;

public interface ICommentMapper
{
    public CommentDto Map(Domain.Comments.Entities.Comment comment, Domain.Users.Entities.User? user);

}