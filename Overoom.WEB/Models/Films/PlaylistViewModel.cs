namespace Overoom.WEB.Models.Films;

public class PlaylistViewModel
{
    public PlaylistViewModel(Guid id, string name, IReadOnlyCollection<string> genres, string description, Uri posterUri, DateTime updated)
    {
        Id = id;
        Name = name;
        Genres = genres;
        Description = description;
        PosterUri = posterUri;
        Updated = updated;
    }

    public Guid Id { get; }
    public string Name { get; }
    public IReadOnlyCollection<string> Genres { get; }
    public string Description { get; }
    public Uri PosterUri { get; }
    public DateTime Updated { get; }
}