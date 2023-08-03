namespace Overoom.Application.Abstractions.MovieApi.Exceptions;

public class ApiNotFoundException : ApiException
{
    public ApiNotFoundException() : base("The content you requested was not found", null)
    {
    }
}