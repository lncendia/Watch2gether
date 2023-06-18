namespace Overoom.Application.Abstractions.Comments.Exceptions;

public class CommentNotBelongToUserException : Exception
{
    public CommentNotBelongToUserException() : base("Comment does not belong to user")
    {
    }
}