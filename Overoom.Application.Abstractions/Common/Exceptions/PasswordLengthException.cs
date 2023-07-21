namespace Overoom.Application.Abstractions.Common.Exceptions;

public class PasswordLengthException : Exception
{
    public PasswordLengthException() : base("Password length should be from 8 to 30 characters")
    {
    }
}