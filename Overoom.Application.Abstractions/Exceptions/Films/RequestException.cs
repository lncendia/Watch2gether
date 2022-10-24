namespace Overoom.Application.Abstractions.Exceptions.Films;

public class RequestException : Exception
{
    public RequestException(Exception? innerEx = null) : base($"Failed to send request.", innerEx)
    {
    }
}