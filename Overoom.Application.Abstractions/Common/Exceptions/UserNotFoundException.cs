namespace Overoom.Application.Abstractions.Common.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("Can't find user")
    {
    }
}