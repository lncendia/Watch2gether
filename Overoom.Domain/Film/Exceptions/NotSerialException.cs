namespace Overoom.Domain.Film.Exceptions;

public class NotSerialException : Exception
{
    public NotSerialException() : base("This is not a serial.")
    {
    }
}