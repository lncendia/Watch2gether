using Room.Domain.Abstractions.Interfaces;
using Room.Domain.YoutubeRooms;

namespace Room.Domain.Abstractions.Repositories;

public interface IYoutubeRoomRepository : IRepository<YoutubeRoom, Guid>;