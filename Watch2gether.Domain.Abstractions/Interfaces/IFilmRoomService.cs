using Watch2gether.Domain.Rooms.FilmRoom;

namespace Watch2gether.Domain.Abstractions.Interfaces;

public interface IFilmRoomService
{
    public Task ChangeSeasonAsync(FilmRoom room, Guid viewerId, int season);
    public Task ChangeSeriesAsync(FilmRoom room, Guid viewerId, int series);
}