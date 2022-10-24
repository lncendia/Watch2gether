namespace Overoom.Application.Abstractions.Exceptions.Films;

public class PosterSaveException : Exception
{
    public PosterSaveException(Exception ex) : base("Failed to save poster", ex)
    {
    }
}