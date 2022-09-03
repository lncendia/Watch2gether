namespace Watch2gether.Application.Abstractions.DTO.Playlists;

public class PlaylistDto
{
    public PlaylistDto(Guid id, string posterFileName, DateTime updated, List<PlaylistFilmLiteDto> films, string name, string description)
    {
        Id = id;
        PosterFileName = posterFileName;
        Updated = updated;
        Films = films;
        Name = name;
        Description = description;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string PosterFileName { get; }
    public DateTime Updated { get; }
    public List<PlaylistFilmLiteDto> Films { get; }
}