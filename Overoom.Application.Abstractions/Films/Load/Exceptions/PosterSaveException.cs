namespace Overoom.Application.Abstractions.Films.Load.Exceptions;

public class PosterSaveException : Exception
{
    public PosterSaveException(Exception ex) : base("Failed to save poster", ex)
    {
    }
}