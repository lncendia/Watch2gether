namespace Overoom.Application.Abstractions.Film.Catalog.Exceptions;

public class FilmNotFoundException : Exception
{
    public FilmNotFoundException() : base("Can't find film.")
    {
    }
}