namespace Room.Infrastructure.Web.Hubs.Models;

public class FilmViewerModel(
    int id,
    string username,
    Uri avatar,
    int time,
    int season,
    int series,
    bool allowBeep,
    bool allowScream,
    bool allowChange)
    : ViewerModel(id, username, avatar, time, allowBeep, allowScream, allowChange)
{
    public int Season { get; } = season;
    public int Series { get; } = series;
}