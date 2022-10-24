namespace Overoom.Application.Abstractions.DTO.Playlists;

public class PlaylistLiteDto
{
    public PlaylistLiteDto(Guid id, string name, string posterFileName, int filmsCount, DateTime updated)
    {
        Id = id;
        Name = name;
        PosterFileName = posterFileName;
        FilmsCount = filmsCount;
        Updated = updated;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string PosterFileName { get; }
    public int FilmsCount { get; }
    public DateTime Updated { get; }
}