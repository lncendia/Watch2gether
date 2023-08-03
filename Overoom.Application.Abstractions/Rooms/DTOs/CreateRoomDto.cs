namespace Overoom.Application.Abstractions.Rooms.DTOs;

public class CreateRoomDto
{
    public CreateRoomDto(bool isOpen)
    {
        IsOpen = isOpen;
    }

    public bool IsOpen { get; }
}