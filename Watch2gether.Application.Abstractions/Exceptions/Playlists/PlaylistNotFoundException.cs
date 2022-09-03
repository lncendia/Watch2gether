namespace Watch2gether.Application.Abstractions.Exceptions.Playlists;

public class PlaylistNotFoundException : Exception
{
    public PlaylistNotFoundException() : base("Can't find playlist.")
    {
    }
}