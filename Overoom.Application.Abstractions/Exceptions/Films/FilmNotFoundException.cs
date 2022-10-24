namespace Overoom.Application.Abstractions.Exceptions.Films;

public class FilmNotFoundException : Exception
{
    public FilmNotFoundException() : base("Can't find film.")
    {
    }
}