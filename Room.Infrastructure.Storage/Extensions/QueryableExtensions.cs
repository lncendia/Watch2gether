using Microsoft.EntityFrameworkCore;
using Room.Infrastructure.Storage.Models.FilmRoom;
using Room.Infrastructure.Storage.Models.YoutubeRoom;

namespace Room.Infrastructure.Storage.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<FilmRoomModel> LoadDependencies(this IQueryable<FilmRoomModel> queryable)
    {
        return queryable
            .Include(r => r.Viewers)
            .Include(r => r.Messages);
    }

    public static IQueryable<YoutubeRoomModel> LoadDependencies(this IQueryable<YoutubeRoomModel> queryable)
    {
        return queryable
            .Include(r => r.Videos)
            .Include(r => r.Viewers)
            .Include(r => r.Messages);
    }
}