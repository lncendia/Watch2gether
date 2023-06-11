namespace Overoom.Application.Abstractions.User.Exceptions;

public class InvalidPasswordFormatException : Exception
{
    public InvalidPasswordFormatException() : base($"Specified password is invalid")
    {
    }
}