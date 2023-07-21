namespace Overoom.Application.Abstractions.Common.Exceptions;

public class FilmNotFoundException : Exception
{
    public FilmNotFoundException() : base("Can't find film.")
    {
    }
}