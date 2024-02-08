using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Rooms.FilmRoom.Entities;
using Room.Domain.Rooms.YoutubeRoom.Entities;

namespace Room.Domain.Abstractions.Repositories;

public interface IYoutubeRoomRepository : IRepository<YoutubeRoom, Guid>;