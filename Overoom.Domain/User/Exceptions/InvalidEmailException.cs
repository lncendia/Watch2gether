namespace Overoom.Domain.User.Exceptions;

public class InvalidEmailException : Exception
{
    public InvalidEmailException(string email) : base($"Invalid email: {email}. Email must be in format: <user>@<domain>.")
    {
    }
}