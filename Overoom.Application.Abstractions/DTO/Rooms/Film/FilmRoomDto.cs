namespace Overoom.Application.Abstractions.DTO.Rooms.Film;

public class FilmRoomDto : RoomDto
{
    public FilmDataDto Film { get; }
    public new List<FilmViewerDto> Viewers => base.Viewers.Cast<FilmViewerDto>().ToList();
    public new List<FilmMessageDto> Messages => base.Messages.Cast<FilmMessageDto>().ToList();

    public FilmRoomDto(FilmDataDto data, IEnumerable<FilmMessageDto> messages, IEnumerable<FilmViewerDto> viewers,
        Guid ownerId) : base(messages, viewers, ownerId) => Film = data;
}