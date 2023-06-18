namespace Overoom.Application.Abstractions.Users.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException() : base("Invalid password entered")
    {
    }
}