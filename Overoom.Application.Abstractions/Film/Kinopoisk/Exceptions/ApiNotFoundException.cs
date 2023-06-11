namespace Overoom.Application.Abstractions.Film.Kinopoisk.Exceptions;

public class ApiNotFoundException : ApiException
{
    public ApiNotFoundException() : base("The content you requested was not found", null)
    {
    }
}