namespace Overoom.Application.Abstractions.Common.Exceptions;

public class PosterSaveException : Exception
{
    public PosterSaveException(Exception ex) : base("Failed to save poster", ex)
    {
    }
}