namespace Overoom.Domain.Comment.Exceptions;

public class TextLengthException : Exception
{
    public TextLengthException() : base("Text length must be between 1 and 1000 characters")
    {
    }
}