namespace Overoom.Application.Abstractions.Comments.Exceptions;

public class CommentNotFoundException : Exception
{
    public CommentNotFoundException() : base("Can't find comment.")
    {
    }
}