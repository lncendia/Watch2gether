namespace Overoom.Application.Abstractions.Room.DTOs.Film;

public class FilmRoomDto : RoomDto
{
    public FilmDataDto Film { get; }
    public new IReadOnlyCollection<FilmViewerDto> Viewers => base.Viewers.Cast<FilmViewerDto>().ToList();
    public new IReadOnlyCollection<FilmMessageDto> Messages => base.Messages.Cast<FilmMessageDto>().ToList();

    public FilmRoomDto(FilmDataDto data, IReadOnlyCollection<FilmMessageDto> messages,
        IReadOnlyCollection<FilmViewerDto> viewers, int ownerId) : base(messages, viewers, ownerId) => Film = data;
}