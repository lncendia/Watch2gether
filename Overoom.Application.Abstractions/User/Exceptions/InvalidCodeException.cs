namespace Overoom.Application.Abstractions.User.Exceptions;

public class InvalidCodeException : Exception
{
    public InvalidCodeException() : base("Invalid code specified")
    {
    }
}