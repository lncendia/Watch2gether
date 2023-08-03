namespace Overoom.Application.Abstractions.Rooms.DTOs.Film;

public class FilmRoomDto : RoomDto
{
    public FilmDataDto Film { get; }
    public new IReadOnlyCollection<FilmViewerDto> Viewers => base.Viewers.Cast<FilmViewerDto>().ToList();

    public FilmRoomDto(FilmDataDto data, IReadOnlyCollection<MessageDto> messages,
        IReadOnlyCollection<FilmViewerDto> viewers, int ownerId, bool isOpen) : base(messages, viewers, ownerId, isOpen) => Film = data;
}