using Room.Domain.BaseRoom.Exceptions;
using Room.Domain.BaseRoom.ValueObjects;

namespace Room.Domain.BaseRoom.Entities;

public abstract class Viewer
{
    public required Guid Id { get; init; }
    public bool Online { get; internal set; } = true;
    public bool FullScreen { get; internal set; }
    public bool Pause { get; internal set; } = true;
    public TimeSpan TimeLine { get; internal set; } = TimeSpan.Zero;

    public required Allows Allows { get; init; }

    public Uri? PhotoUrl { get; init; }

    private string _name = null!;

    public required string Nickname
    {
        get => _name;
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length > 40) throw new InvalidNicknameLengthException();
            _name = value;
        }
    }
}