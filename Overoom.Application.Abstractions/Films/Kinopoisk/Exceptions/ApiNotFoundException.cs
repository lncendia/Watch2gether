namespace Overoom.Application.Abstractions.Films.Kinopoisk.Exceptions;

public class ApiNotFoundException : ApiException
{
    public ApiNotFoundException() : base("The content you requested was not found", null)
    {
    }
}