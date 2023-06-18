namespace Overoom.Domain.Films.Exceptions;

public class ShortDescriptionTooLongException : Exception
{
    public ShortDescriptionTooLongException() : base("The short description of the movie cannot be longer than 500 characters")
    {
    }
}