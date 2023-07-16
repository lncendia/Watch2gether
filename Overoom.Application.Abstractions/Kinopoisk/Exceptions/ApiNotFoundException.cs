namespace Overoom.Application.Abstractions.Kinopoisk.Exceptions;

public class ApiNotFoundException : ApiException
{
    public ApiNotFoundException() : base("The content you requested was not found", null)
    {
    }
}