namespace Overoom.Domain.Playlists.Exceptions;

public class NameLengthException : Exception
{
    public NameLengthException() : base("The playlist name length must be between 1 and 200 characters")
    {
    }
}