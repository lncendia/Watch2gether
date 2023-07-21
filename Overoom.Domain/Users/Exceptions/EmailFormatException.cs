namespace Overoom.Domain.Users.Exceptions;

public class EmailFormatException : Exception
{
    public EmailFormatException() : base("Email must be in format: <user>@<domain>.")
    {
    }
}