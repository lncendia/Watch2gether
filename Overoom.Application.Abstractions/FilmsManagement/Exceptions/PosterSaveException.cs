namespace Overoom.Application.Abstractions.FilmsManagement.Exceptions;

public class PosterSaveException : Exception
{
    public PosterSaveException(Exception ex) : base("Failed to save poster", ex)
    {
    }
}