namespace Overoom.Domain.Film.Exceptions;

public class EmptyTagsCollectionException : Exception
{
    public EmptyTagsCollectionException() : base("The tag collection cannot be empty")
    {
    }
}