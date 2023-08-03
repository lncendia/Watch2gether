namespace Overoom.Application.Abstractions.Rooms.DTOs.Youtube;

public class CreateYoutubeRoomDto : CreateRoomDto
{
    public CreateYoutubeRoomDto(bool isOpen, Uri url, bool addAccess) : base(isOpen)
    {
        Url = url;
        AddAccess = addAccess;
    }

    public Uri Url { get; }
    public bool AddAccess { get; }
}