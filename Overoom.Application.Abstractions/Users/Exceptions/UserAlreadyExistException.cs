namespace Overoom.Application.Abstractions.Users.Exceptions;

public class UserAlreadyExistException : Exception
{
    public UserAlreadyExistException() : base("The user is already registered")
    {
    }
}