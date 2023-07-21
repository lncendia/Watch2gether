namespace Overoom.Application.Abstractions.Authentication.Exceptions;

public class UserAlreadyExistException : Exception
{
    public UserAlreadyExistException() : base("The user is already registered")
    {
    }
}