using Watch2gether.Domain.Rooms;
using Watch2gether.Domain.Rooms.FilmRoom;

namespace Watch2gether.Domain.Abstractions.Interfaces;

public interface IFilmRoomService
{
    public void ChangeSeason(FilmRoom room, Guid viewerId, int season);
    public void ChangeSeries(FilmRoom room, Guid viewerId, int series);
}