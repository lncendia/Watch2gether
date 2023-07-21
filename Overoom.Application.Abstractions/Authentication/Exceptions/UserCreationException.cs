namespace Overoom.Application.Abstractions.Authentication.Exceptions;

public class UserCreationException : Exception
{
    public UserCreationException(string error) : base($"Failed to create user: {error}")
    {
    }
}