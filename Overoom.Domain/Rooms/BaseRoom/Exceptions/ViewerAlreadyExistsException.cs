namespace Overoom.Domain.Rooms.BaseRoom.Exceptions;

public class ViewerAlreadyExistsException : Exception
{
    public ViewerAlreadyExistsException() : base("Viewer already exists")
    {
    }
}