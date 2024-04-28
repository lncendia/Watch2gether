using Room.Domain.Extensions;
using Room.Domain.Rooms.Rooms.ValueObjects;

namespace Room.Domain.Rooms.Rooms.Entities;

public abstract class Viewer
{
    private const int MaxUsernameLength = 40;

    public required Guid Id { get; init; }
    public bool Online { get; internal set; }
    public bool FullScreen { get; internal set; }
    public bool Pause { get; internal set; } = true;
    public TimeSpan TimeLine { get; internal set; } = TimeSpan.Zero;

    public required Allows Allows { get; init; }

    public Uri? PhotoUrl { get; init; }

    private string _name = null!;

    public required string Username
    {
        get => _name;
        set => _name = value.ValidateLength(MaxUsernameLength);
    }
}