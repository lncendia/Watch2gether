namespace Overoom.Application.Abstractions.Users.Exceptions;

public class InvalidCodeException : Exception
{
    public InvalidCodeException() : base("Invalid code specified")
    {
    }
}