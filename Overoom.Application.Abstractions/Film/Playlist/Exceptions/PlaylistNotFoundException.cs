namespace Overoom.Application.Abstractions.Film.Playlist.Exceptions;

public class PlaylistNotFoundException : Exception
{
    public PlaylistNotFoundException() : base("Can't find playlist.")
    {
    }
}