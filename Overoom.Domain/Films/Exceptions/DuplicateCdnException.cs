namespace Overoom.Domain.Films.Exceptions;

public class DuplicateCdnException : Exception
{
    public DuplicateCdnException() : base("There cannot be two CDNs of the same type")
    {
    }
}