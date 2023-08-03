namespace Overoom.Application.Abstractions.Rooms.DTOs.Film;

public class FilmViewerDto : ViewerDto
{
    public FilmViewerDto(string username, int id, Uri avatarUrl, TimeSpan time, bool pause, bool fullScreen, int season,
        int series, AllowsDto allows) : base(username, id, avatarUrl, time, pause, fullScreen, allows)
    {
        Season = season;
        Series = series;
    }

    public int Season { get; }
    public int Series { get; }
}