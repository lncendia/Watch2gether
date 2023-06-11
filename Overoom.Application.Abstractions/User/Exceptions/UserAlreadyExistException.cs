namespace Overoom.Application.Abstractions.User.Exceptions;

public class UserAlreadyExistException : Exception
{
    public UserAlreadyExistException() : base("The user is already registered")
    {
    }
}