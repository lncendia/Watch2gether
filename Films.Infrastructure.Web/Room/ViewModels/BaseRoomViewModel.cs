namespace Films.Infrastructure.Web.Models.Rooms;

public abstract class BaseRoomViewModel
{
    protected BaseRoomViewModel(IReadOnlyCollection<MessageViewModel> messages,
        IReadOnlyCollection<ViewerViewModel> viewers, string connectUrl, int ownerId, int currentViewerId, bool isOpen)
    {
        Messages = messages;
        ConnectUrl = connectUrl;
        OwnerId = ownerId;
        Viewers = viewers;
        CurrentViewerId = currentViewerId;
        IsOpen = isOpen;
    }

    public IReadOnlyCollection<MessageViewModel> Messages { get; }
    protected readonly IReadOnlyCollection<ViewerViewModel> Viewers;

    public int CurrentViewerId { get; }
    public int OwnerId { get; }
    public string ConnectUrl { get; }
    public bool IsOpen { get; }
}