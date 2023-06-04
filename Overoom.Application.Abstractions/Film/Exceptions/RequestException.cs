namespace Overoom.Application.Abstractions.Film.Exceptions;

public class RequestException : Exception
{
    public RequestException(Exception? innerEx = null) : base($"Failed to send request.", innerEx)
    {
    }
}