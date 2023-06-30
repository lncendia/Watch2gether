namespace Overoom.Application.Abstractions.Films.Playlist.DTOs;

public class PlaylistShortDto
{
    public PlaylistShortDto(Guid id, Uri posterUri, string name)
    {
        Id = id;
        PosterUri = posterUri;
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; }
    public Uri PosterUri { get; }
}