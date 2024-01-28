namespace Films.Infrastructure.Web.Models.Playlists;

public class PlaylistViewModel
{
    public PlaylistViewModel(Guid id, string name, IEnumerable<string> genres, string description, Uri posterUri,
        DateTime updated)
    {
        Id = id;
        Name = name;
        Genres = string.Join(", ", genres);
        Description = description;
        PosterUri = posterUri;
        Updated = updated.ToString("dd.MM.yyyy");
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Genres { get; }
    public string Description { get; }
    public Uri PosterUri { get; }
    public string Updated { get; }
}