using Room.Domain.Rooms.Rooms.Exceptions;
using Room.Domain.Rooms.Rooms.ValueObjects;

namespace Room.Domain.Rooms.Rooms.Entities;

public abstract class Viewer
{
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
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length > 40) throw new InvalidUsernameLengthException();
            _name = value;
        }
    }
}