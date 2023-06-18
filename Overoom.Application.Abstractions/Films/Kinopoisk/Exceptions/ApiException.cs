namespace Overoom.Application.Abstractions.Films.Kinopoisk.Exceptions;

public class ApiException : Exception
{
    public ApiException(string message, Exception? inner) : base(message, inner)
    {
    }
}