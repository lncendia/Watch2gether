namespace Watch2gether.WEB.Hubs.Models;

public class FilmViewerModel : ViewerModel
{
    public FilmViewerModel(Guid id, string username, string avatar, int time, int season, int series) : base(id,
        username, avatar, time)
    {
        Season = season;
        Series = series;
    }

    public int Season { get; }
    public int Series { get; }
}