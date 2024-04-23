namespace Films.Infrastructure.Web.Models.Rooms.FilmRoom;

public class FilmViewerViewModel : ViewerViewModel
{
    public int Season { get; }
    public int Series { get; }

    public FilmViewerViewModel(int id, string username, Uri AvatarUrl, bool pause, TimeSpan time, int season,
        int series, bool fullScreen, bool allowBeep, bool allowScream, bool allowChange) : base(id, username, AvatarUrl,
        pause, time, fullScreen, allowBeep, allowScream, allowChange)
    {
        Season = season;
        Series = series;
    }
}