namespace Overoom.Domain.Playlists.Exceptions;

public class DescriptionLengthException : Exception
{
    public DescriptionLengthException() : base("The playlist description length must be between 1 and 500 characters")
    {
    }
}