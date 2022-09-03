namespace Watch2gether.Application.Abstractions.Exceptions.Users;

public class EmailException : Exception
{
    public EmailException(Exception baseException) : base("Failed to send email.", baseException)
    {
    }
}