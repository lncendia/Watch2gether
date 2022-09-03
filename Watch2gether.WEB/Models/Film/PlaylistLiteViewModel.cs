namespace Watch2gether.WEB.Models.Film;

public class PlaylistLiteViewModel
{
    public PlaylistLiteViewModel(Guid id, string name, string posterFileName)
    {
        Id = id;
        Name = name;
        PosterFileName = posterFileName;
    }

    public Guid Id { get; }
    public string Name { get;}
    public string PosterFileName { get; }
}