using Overoom.Application.Abstractions.Comment.DTOs;

namespace Overoom.Application.Abstractions.Comment.Interfaces;

public interface ICommentMapper
{
    public CommentDto Map(Domain.Comment.Entities.Comment comment, Domain.User.Entities.User? user);

}