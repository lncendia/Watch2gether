namespace Watch2gether.Domain.Films.Exceptions;

public class NotSerialException : Exception
{
    public NotSerialException() : base("This is not a serial.")
    {
    }
}