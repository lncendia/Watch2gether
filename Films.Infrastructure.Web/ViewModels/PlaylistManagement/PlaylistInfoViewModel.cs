namespace Films.Infrastructure.Web.Models.PlaylistManagement;

public class PlaylistInfoViewModel
{
    public PlaylistInfoViewModel(Guid id, Uri posterUri, string name, string description,
        IReadOnlyCollection<FilmViewModel> films, IReadOnlyCollection<string> genres)
    {
        Id = id;
        PosterUri = posterUri;
        Name = name;
        Films = films;
        Description = description;
        Genres = genres;
    }

    public Guid Id { get; }
    public Uri PosterUri { get; }
    public string Name { get; }
    public string Description { get; }
    public IReadOnlyCollection<string> Genres { get; }
    public IReadOnlyCollection<FilmViewModel> Films { get; }
}