namespace Overoom.Application.Abstractions.Common.Exceptions;

public class RoomNotFoundException : Exception
{
    public RoomNotFoundException() : base("Can't find room.")
    {
    }
}