namespace Overoom.Application.Abstractions.Playlists.Exceptions;

public class PlaylistNotFoundException : Exception
{
    public PlaylistNotFoundException() : base("Can't find playlist.")
    {
    }
}