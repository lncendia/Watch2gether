using Microsoft.EntityFrameworkCore;
using Room.Infrastructure.Storage.Models.FilmRooms;
using Room.Infrastructure.Storage.Models.YoutubeRooms;

namespace Room.Infrastructure.Storage.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<FilmRoomModel> LoadDependencies(this IQueryable<FilmRoomModel> queryable)
    {
        return queryable
            .Include(r => r.Viewers);
    }

    public static IQueryable<YoutubeRoomModel> LoadDependencies(this IQueryable<YoutubeRoomModel> queryable)
    {
        return queryable
            .Include(r => r.Videos)
            .Include(r => r.Viewers);
    }
}