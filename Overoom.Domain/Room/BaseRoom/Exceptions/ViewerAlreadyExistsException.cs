namespace Overoom.Domain.Room.BaseRoom.Exceptions;

public class ViewerAlreadyExistsException : Exception
{
    public ViewerAlreadyExistsException() : base("Viewer already exists")
    {
    }
}