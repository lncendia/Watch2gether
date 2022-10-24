namespace Overoom.Application.Abstractions.Exceptions.Users;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("Can't find user.")
    {
    }
}