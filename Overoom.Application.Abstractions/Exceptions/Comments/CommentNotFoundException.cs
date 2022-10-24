namespace Overoom.Application.Abstractions.Exceptions.Comments;

public class CommentNotFoundException : Exception
{
    public CommentNotFoundException() : base("Can't find comment.")
    {
    }
}