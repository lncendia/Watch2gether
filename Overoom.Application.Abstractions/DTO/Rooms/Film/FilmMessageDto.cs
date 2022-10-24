namespace Overoom.Application.Abstractions.DTO.Rooms.Film;

public class FilmMessageDto : MessageDto
{
    public new FilmViewerDto Viewer => (FilmViewerDto) base.Viewer;

    public FilmMessageDto(string text, DateTime createdAt, FilmViewerDto viewer) : base(text, createdAt, viewer)
    {
    }
}