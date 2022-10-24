namespace Overoom.Application.Abstractions.Exceptions.Comments;

public class CommentNotBelongToUserException : Exception
{
    public CommentNotBelongToUserException() : base("Comment does not belong to user")
    {
    }
}