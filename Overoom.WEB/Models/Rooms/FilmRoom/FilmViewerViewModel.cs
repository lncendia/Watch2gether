namespace Overoom.WEB.Models.Rooms.FilmRoom;

public class FilmViewerViewModel : ViewerViewModel
{
    public int Season { get; }
    public int Series { get; }
    
    public FilmViewerViewModel(int id, string username, Uri avatarUri, bool onPause, TimeSpan time, int season,
        int series) : base(id, username, avatarUri, onPause, time)
    {
        Season = season;
        Series = series;
    }
}