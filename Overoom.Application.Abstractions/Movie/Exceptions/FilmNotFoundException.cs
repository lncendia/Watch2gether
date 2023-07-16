namespace Overoom.Application.Abstractions.Movie.Exceptions;

public class FilmNotFoundException : Exception
{
    public FilmNotFoundException() : base("Can't find film.")
    {
    }
}