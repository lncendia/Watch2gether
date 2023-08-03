namespace Overoom.Application.Abstractions.PlaylistsManagement.DTOs;

public class PlaylistShortDto
{
    public PlaylistShortDto(Guid id, string name, Uri posterUri)
    {
        Id = id;
        Name = name;
        PosterUri = posterUri;
    }

    public Guid Id { get; }
    public string Name { get; }
    public Uri PosterUri { get; }
}