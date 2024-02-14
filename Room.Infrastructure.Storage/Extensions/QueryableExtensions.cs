using Microsoft.EntityFrameworkCore;
using Room.Infrastructure.Storage.Models.Film;
using Room.Infrastructure.Storage.Models.Room.FilmRoom;
using Room.Infrastructure.Storage.Models.Room.YoutubeRoom;

namespace Room.Infrastructure.Storage.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<FilmRoomModel> LoadDependencies(this IQueryable<FilmRoomModel> queryable)
    {
        return queryable
            .Include(r => r.BannedUsers)
            .Include(r => r.Viewers)
            .Include(r => r.Messages);
    }

    public static IQueryable<YoutubeRoomModel> LoadDependencies(this IQueryable<YoutubeRoomModel> queryable)
    {
        return queryable
            .Include(r => r.Videos)
            .Include(r => r.BannedUsers)
            .Include(r => r.Viewers)
            .Include(r => r.Messages);
    }

    public static IQueryable<FilmModel> LoadDependencies(this IQueryable<FilmModel> queryable)
    {
        return queryable.Include(f => f.CdnList);
    }
}