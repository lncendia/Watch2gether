namespace Overoom.Application.Abstractions.Exceptions.Users;

public class UserCreationException : Exception
{
    public UserCreationException(string error) : base($"Failed to create user: {error}.")
    {
    }
}