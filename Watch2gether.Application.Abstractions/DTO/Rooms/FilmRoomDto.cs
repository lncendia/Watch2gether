namespace Watch2gether.Application.Abstractions.DTO.Rooms;

public class FilmRoomDto : RoomDto
{
    public FilmDataDto Film { get; }

    public FilmRoomDto(FilmDataDto data, List<MessageDto> messages, List<ViewerDto> viewers, Guid ownerId) : base(
        messages, viewers, ownerId) =>
        Film = data;
}