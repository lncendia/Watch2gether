namespace Overoom.Application.Abstractions.Common.Exceptions;

public class InvalidCodeException : Exception
{
    public InvalidCodeException() : base("Invalid code specified")
    {
    }
}