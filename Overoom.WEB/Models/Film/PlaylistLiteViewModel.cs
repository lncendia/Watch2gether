namespace Overoom.WEB.Models.Film;

public class PlaylistLiteViewModel
{
    public PlaylistLiteViewModel(Guid id, string name, string posterUri)
    {
        Id = id;
        Name = name;
        PosterUri = posterUri;
    }

    public Guid Id { get; }
    public string Name { get;}
    public string PosterUri { get; }
}