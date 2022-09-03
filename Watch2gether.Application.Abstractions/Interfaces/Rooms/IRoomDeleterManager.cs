namespace Watch2gether.Application.Abstractions.Interfaces.Rooms;

public interface IRoomDeleterManager
{
    Task DeleteAll();
    Task DeleteRoomIfEmpty(Guid roomId);
}