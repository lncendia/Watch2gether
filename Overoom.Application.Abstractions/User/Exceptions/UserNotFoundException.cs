namespace Overoom.Application.Abstractions.User.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("Can't find user")
    {
    }
}