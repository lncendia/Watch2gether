namespace Overoom.Domain.Films.Exceptions;

public class ShortDescriptionLengthException : Exception
{
    public ShortDescriptionLengthException() : base("The movie short description length must be between 1 and 500 characters")
    {
    }
}