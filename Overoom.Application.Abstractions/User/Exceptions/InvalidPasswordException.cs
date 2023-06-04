namespace Overoom.Application.Abstractions.User.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException() : base("Invalid password entered.")
    {
    }
}