using Overoom.Application.Abstractions.Exceptions.Rooms;
using Overoom.Application.Abstractions.Interfaces.Rooms;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;

namespace Overoom.Application.Services.Services.Rooms;

public class RoomDeleterManager : IRoomDeleterManager
{
    private readonly IUnitOfWork _unitOfWork;

    public RoomDeleterManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteAll()
    {
        await _unitOfWork.FilmRoomRepository.Value.DeleteRangeAsync(
            await _unitOfWork.FilmRoomRepository.Value.FindAsync(null));
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteRoomIfEmpty(Guid roomId)
    {
        var room = await _unitOfWork.FilmRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        if (room.Viewers.Any(x => x.Online)) return;
        await _unitOfWork.FilmRoomRepository.Value.DeleteAsync(room);
        await _unitOfWork.SaveAsync();
    }
}