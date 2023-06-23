namespace Overoom.Application.Abstractions.Rooms.DTOs.Film;

public class FilmViewerDto : ViewerDto
{
    public FilmViewerDto(string username, int id, Uri avatarUrl, TimeSpan time, bool onPause, int season,
        int series) : base(username, id, avatarUrl, time, onPause)
    {
        Season = season;
        Series = series;
    }

    public int Season { get; }
    public int Series { get; }
}