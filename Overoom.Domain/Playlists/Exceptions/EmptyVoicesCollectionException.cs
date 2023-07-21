namespace Overoom.Domain.Playlists.Exceptions;

public class EmptyGenresCollectionException : Exception
{
    public EmptyGenresCollectionException() : base("The collection of genres cannot be empty")
    {
    }
}