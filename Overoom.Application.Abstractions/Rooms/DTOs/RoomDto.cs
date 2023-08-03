namespace Overoom.Application.Abstractions.Rooms.DTOs;

public abstract class RoomDto
{
    protected RoomDto(IReadOnlyCollection<MessageDto> messages, IReadOnlyCollection<ViewerDto> viewers, int ownerId, bool isOpen)
    {
        Messages = messages;
        Viewers = viewers;
        OwnerId = ownerId;
        IsOpen = isOpen;
    }

    public int OwnerId { get; }
    public bool IsOpen { get; }
    public IReadOnlyCollection<MessageDto> Messages { get; }
    protected readonly IReadOnlyCollection<ViewerDto> Viewers;
}