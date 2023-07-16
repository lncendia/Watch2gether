namespace Overoom.Domain.Films.Exceptions;

public class NameLengthException : Exception
{
    public NameLengthException() : base("The movie name length must be between 1 and 200 characters")
    {
    }
}