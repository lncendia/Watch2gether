namespace Films.Infrastructure.Web.Models.Film;

public class PlaylistShortViewModel
{
    public PlaylistShortViewModel(Guid id, string name, Uri posterUri)
    {
        Id = id;
        Name = name;
        PosterUri = posterUri;
    }

    public Guid Id { get; }
    public string Name { get; }
    public Uri PosterUri { get; }
}