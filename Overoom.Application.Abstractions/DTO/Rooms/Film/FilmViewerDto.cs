namespace Overoom.Application.Abstractions.DTO.Rooms.Film;

public class FilmViewerDto : ViewerDto
{
    public FilmViewerDto(string username, Guid id, string avatarUrl, TimeSpan time, bool onPause, int season,
        int series) : base(username, id, avatarUrl, time, onPause)
    {
        Season = season;
        Series = series;
    }

    public int Season { get; }
    public int Series { get; }
}