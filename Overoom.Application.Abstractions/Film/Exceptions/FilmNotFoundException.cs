namespace Overoom.Application.Abstractions.Film.Exceptions;

public class FilmNotFoundException : Exception
{
    public FilmNotFoundException() : base("Can't find film.")
    {
    }
}