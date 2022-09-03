namespace Watch2gether.Domain.Rooms.Exceptions;

public class ViewerNotFoundException : Exception
{
    public ViewerNotFoundException() : base($"A viewer is not found in this room.")
    {
    }
}