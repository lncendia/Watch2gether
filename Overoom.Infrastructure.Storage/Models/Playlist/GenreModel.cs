namespace Overoom.Infrastructure.Storage.Models.Playlist;

public class PlaylistGenreModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    
    public PlaylistModel PlaylistModel { get; set; } = null!;
}