namespace Overoom.Application.Abstractions.Film.DTOs.Playlist;

public class PlaylistDto
{
    public PlaylistDto(Guid id, string posterUri, DateTime updated, string name, string description)
    {
        Id = id;
        PosterUri = posterUri;
        Updated = updated;
        Name = name;
        Description = description;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string PosterUri { get; }
    public DateTime Updated { get; }
}