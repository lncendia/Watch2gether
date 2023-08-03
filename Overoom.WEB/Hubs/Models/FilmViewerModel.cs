namespace Overoom.WEB.Hubs.Models;

public class FilmViewerModel : ViewerModel
{
    public FilmViewerModel(int id, string username, Uri avatar, int time, int season, int series, bool allowBeep, bool allowScream,
        bool allowChange) : base(id, username, avatar, time, allowBeep, allowScream, allowChange)
    {
        Season = season;
        Series = series;
    }

    public int Season { get; }
    public int Series { get; }
}