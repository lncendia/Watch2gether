namespace Overoom.WEB.Models.PlaylistManagement;

public class PlaylistViewModel
{
    public PlaylistViewModel(Guid id, Uri posterUri, string name)
    {
        Id = id;
        PosterUri = posterUri;
        Name = name;
    }

    public Guid Id { get; }
    public Uri PosterUri { get; }
    public string Name { get; }
}