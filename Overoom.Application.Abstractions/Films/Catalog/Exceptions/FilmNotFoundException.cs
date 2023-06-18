namespace Overoom.Application.Abstractions.Films.Catalog.Exceptions;

public class FilmNotFoundException : Exception
{
    public FilmNotFoundException() : base("Can't find film.")
    {
    }
}