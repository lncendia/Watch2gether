namespace Overoom.Application.Abstractions.Common.Exceptions;

public class PasswordFormatException : Exception
{
    public PasswordFormatException() : base("The password must contain letters, numbers and special characters and cannot have breaks" )
    {
    }
}