namespace Overoom.Application.Abstractions.Films.Playlist.Exceptions;

public class PlaylistNotFoundException : Exception
{
    public PlaylistNotFoundException() : base("Can't find playlist.")
    {
    }
}