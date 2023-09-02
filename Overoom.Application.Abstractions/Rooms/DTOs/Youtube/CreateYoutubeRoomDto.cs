namespace Overoom.Application.Abstractions.Rooms.DTOs.Youtube;

public class CreateYoutubeRoomDto : CreateRoomDto
{
    public CreateYoutubeRoomDto(bool isOpen, Uri url, bool access) : base(isOpen)
    {
        Url = url;
        Access = access;
    }

    public Uri Url { get; }
    public bool Access { get; }
}