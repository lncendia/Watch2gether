namespace Room.Infrastructure.Web.Rooms.Exceptions;

public class ActionCooldownException(int seconds) : Exception($"The action will be available in {seconds} seconds")
{
    public int Seconds { get; } = seconds;
}