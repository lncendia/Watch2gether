namespace Overoom.Application.Abstractions.Exceptions.Playlists;

public class PlaylistNotFoundException : Exception
{
    public PlaylistNotFoundException() : base("Can't find playlist.")
    {
    }
}