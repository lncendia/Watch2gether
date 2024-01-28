namespace Films.Domain.Users.ValueObjects;

public class Allows
{
    public required bool Beep { get; init; }
    public required bool Scream { get; init; }
    public required bool Change { get; init; }
}