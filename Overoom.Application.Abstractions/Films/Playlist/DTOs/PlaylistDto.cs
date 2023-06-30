namespace Overoom.Application.Abstractions.Films.Playlist.DTOs;

public class PlaylistDto
{
    public PlaylistDto(Guid id, Uri posterUri, DateTime updated, string name, string description,
        IReadOnlyCollection<string> genres)
    {
        Id = id;
        PosterUri = posterUri;
        Updated = updated;
        Name = name;
        Description = description;
        Genres = genres;
    }

    public Guid Id { get; }
    public string Name { get; }
    public IReadOnlyCollection<string> Genres { get; }
    public string Description { get; }
    public Uri PosterUri { get; }
    public DateTime Updated { get; }
}