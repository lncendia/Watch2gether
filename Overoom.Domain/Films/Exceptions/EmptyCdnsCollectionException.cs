namespace Overoom.Domain.Films.Exceptions;

public class EmptyCdnsCollectionException : Exception
{
    public EmptyCdnsCollectionException() : base("The cdns collection cannot be empty")
    {
    }
}