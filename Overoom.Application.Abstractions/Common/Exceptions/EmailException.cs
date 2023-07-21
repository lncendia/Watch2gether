namespace Overoom.Application.Abstractions.Common.Exceptions;

public class EmailException : Exception
{
    public EmailException(Exception baseException) : base("Failed to send email.", baseException)
    {
    }
}