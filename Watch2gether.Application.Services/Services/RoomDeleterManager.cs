using Watch2gether.Application.Abstractions.Exceptions.Rooms;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.Domain.Abstractions.Repositories;

namespace Watch2gether.Application.Services.Services;

public class RoomDeleterManager : IRoomDeleterManager
{
    private readonly IUnitOfWork _unitOfWork;

    public RoomDeleterManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteAll()
    {
        await _unitOfWork.RoomRepository.Value.DeleteRangeAsync(await _unitOfWork.RoomRepository.Value.FindAsync(null));
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteRoomIfEmpty(Guid roomId)
    {
        var room = await _unitOfWork.RoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        if (room.Viewers.Any(x => x.Online)) return;
        await _unitOfWork.RoomRepository.Value.DeleteAsync(room);
        await _unitOfWork.SaveAsync();
    }
}