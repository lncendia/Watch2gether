namespace Overoom.WEB.Models.Films;

public class PlaylistViewModel
{
    public PlaylistViewModel(Guid id, string name, string posterUri)
    {
        Id = id;
        Name = name;
        PosterUri = posterUri;
    }

    public Guid Id { get; }
    public string Name { get;}
    public string PosterUri { get; }
}