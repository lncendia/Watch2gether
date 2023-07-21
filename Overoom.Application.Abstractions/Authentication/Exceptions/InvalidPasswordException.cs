namespace Overoom.Application.Abstractions.Authentication.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException() : base("Invalid password entered")
    {
    }
}