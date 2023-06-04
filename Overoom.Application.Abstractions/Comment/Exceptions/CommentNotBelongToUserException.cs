namespace Overoom.Application.Abstractions.Comment.Exceptions;

public class CommentNotBelongToUserException : Exception
{
    public CommentNotBelongToUserException() : base("Comment does not belong to user")
    {
    }
}