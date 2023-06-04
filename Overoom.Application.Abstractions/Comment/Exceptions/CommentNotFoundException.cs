namespace Overoom.Application.Abstractions.Comment.Exceptions;

public class CommentNotFoundException : Exception
{
    public CommentNotFoundException() : base("Can't find comment.")
    {
    }
}