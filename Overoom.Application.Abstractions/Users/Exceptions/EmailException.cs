namespace Overoom.Application.Abstractions.Users.Exceptions;

public class EmailException : Exception
{
    public EmailException(Exception baseException) : base("Failed to send email.", baseException)
    {
    }
}