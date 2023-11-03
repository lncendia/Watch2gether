using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Users.Entities;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Models.User;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class UserModelMapper : IModelMapperUnit<UserModel, User>
{
    private readonly ApplicationDbContext _context;

    public UserModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<UserModel> MapAsync(User entity)
    {
        var user = _context.Users.Local.FirstOrDefault(x => x.Id == entity.Id);
        if (user == null)
        {
            user = await _context.Users.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (user != null)
            {
                await _context.Entry(user).Collection(x => x.History).LoadAsync();
                await _context.Entry(user).Collection(x => x.Watchlist).LoadAsync();
                await _context.Entry(user).Collection(x => x.Genres).LoadAsync();
            }
            else user = new UserModel { Id = entity.Id };
        }

        user.Name = entity.Name;
        user.NameNormalized = entity.Name.ToUpper();
        user.Email = entity.Email;
        user.EmailNormalized = entity.Email.ToUpper();
        user.AvatarUri = entity.AvatarUri;
        user.Beep = entity.Allows.Beep;
        user.Scream = entity.Allows.Scream;
        user.Change = entity.Allows.Change;


        if (entity.History.Any())
        {
            var newHistory = entity.History.Where(x => user.History.All(m => m.FilmId != x.FilmId || m.Date != x.Date));
            user.History.AddRange(newHistory.Select(x => new HistoryModel { FilmId = x.FilmId, Date = x.Date }));
            _context.UserHistory.RemoveRange(user.History.Where(x =>
                entity.History.All(m => m.FilmId != x.FilmId || m.Date != x.Date)));
        }

        if (entity.Watchlist.Any())
        {
            var newWatchlist =
                entity.Watchlist.Where(x => user.Watchlist.All(m => m.FilmId != x.FilmId || m.Date != x.Date));
            user.Watchlist.AddRange(newWatchlist.Select(x => new WatchlistModel { FilmId = x.FilmId, Date = x.Date }));
            _context.UserWatchlist.RemoveRange(user.Watchlist.Where(x =>
                entity.Watchlist.All(m => m.FilmId != x.FilmId || m.Date != x.Date)));
        }


        if (entity.Genres.Any())
        {
            var newGenres = entity.Genres.Where(x => user.Genres.All(m => m.NameNormalized != x.ToUpper()));
            var removeGenres = user.Genres.Where(x => entity.Genres.All(m => m.ToUpper() != x.NameNormalized));
            var databaseGenres = _context.Genres
                .Where(x => newGenres.Select(s => s.ToUpper()).Any(g => g == x.NameNormalized)).ToList();
            user.Genres.AddRange(databaseGenres);
            user.Genres.RemoveAll(g => removeGenres.Contains(g));
        }

        return user;
    }
}