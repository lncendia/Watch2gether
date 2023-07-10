namespace Overoom.Domain.Films.Exceptions;

public class EmptyVoicesCollectionException : Exception
{
    public EmptyVoicesCollectionException() : base("The collection of voices cannot be empty")
    {
    }
}