namespace Overoom.Application.Abstractions.Movie.Exceptions;

public class FilmAlreadyExistsException : Exception
{
    public FilmAlreadyExistsException() : base("There can't be films with the same title and the same release year")
    {
    }
}