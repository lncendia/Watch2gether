namespace Overoom.Application.Abstractions.Film.DTOs.Playlist;

public class PlaylistDto
{
    public PlaylistDto(Guid id, string posterFileName, DateTime updated, string name, string description)
    {
        Id = id;
        PosterFileName = posterFileName;
        Updated = updated;
        Name = name;
        Description = description;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string PosterFileName { get; }
    public DateTime Updated { get; }
}