namespace Overoom.Application.Abstractions.FilmsManagement.Exceptions;

public class PosterMissingException : Exception
{
    public PosterMissingException() : base("The film can't be without a poster")
    {
    }
}