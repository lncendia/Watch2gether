namespace Watch2gether.WEB.Models.Room.FilmRoom;

public class FilmViewerViewModel : ViewerViewModel
{
    public int Season { get; }
    public int Series { get; }
    
    public FilmViewerViewModel(Guid id, string username, string avatarUrl, bool onPause, TimeSpan time, int season,
        int series) : base(id, username, avatarUrl, onPause, time)
    {
        Season = season;
        Series = series;
    }
}