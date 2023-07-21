namespace Overoom.Application.Abstractions.Common.Exceptions;

public class PosterMissingException : Exception
{
    public PosterMissingException() : base("The film or playlist can't be without a poster")
    {
    }
}