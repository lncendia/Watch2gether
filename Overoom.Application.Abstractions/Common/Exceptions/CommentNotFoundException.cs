namespace Overoom.Application.Abstractions.Common.Exceptions;

public class CommentNotFoundException : Exception
{
    public CommentNotFoundException() : base("Can't find comment.")
    {
    }
}