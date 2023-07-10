namespace Overoom.Domain.Films.Exceptions;

public class SerialException : Exception
{
    public SerialException() : base("For the series, you must specify the number of seasons and episodes")
    {
    }
}