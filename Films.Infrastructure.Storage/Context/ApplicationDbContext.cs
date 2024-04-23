using Films.Infrastructure.Storage.Models.Comments;
using Films.Infrastructure.Storage.Models.Countries;
using Films.Infrastructure.Storage.Models.FilmRooms;
using Films.Infrastructure.Storage.Models.Films;
using Films.Infrastructure.Storage.Models.Genres;
using Films.Infrastructure.Storage.Models.Persons;
using Films.Infrastructure.Storage.Models.Playlists;
using Films.Infrastructure.Storage.Models.Ratings;
using Films.Infrastructure.Storage.Models.Rooms;
using Films.Infrastructure.Storage.Models.Servers;
using Films.Infrastructure.Storage.Models.Users;
using Films.Infrastructure.Storage.Models.YoutubeRoom;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public List<INotification> Notifications { get; } = [];
    public DbSet<UserModel> Users { get; set; } = null!;
    public DbSet<HistoryModel> UserHistory { get; set; } = null!;
    public DbSet<WatchlistModel> UserWatchlist { get; set; } = null!;


    public DbSet<FilmModel> Films { get; set; } = null!;
    public DbSet<CdnModel> FilmCdns { get; set; } = null!;
    public DbSet<FilmActorModel> FilmActors { get; set; } = null!;


    public DbSet<PersonModel> Persons { get; set; } = null!;
    public DbSet<CountryModel> Countries { get; set; } = null!;
    public DbSet<GenreModel> Genres { get; set; } = null!;


    public DbSet<PlaylistModel> Playlists { get; set; } = null!;
    public DbSet<PlaylistFilmModel> PlaylistFilms { get; set; } = null!;

    public DbSet<CommentModel> Comments { get; set; } = null!;

    public DbSet<RatingModel> Ratings { get; set; } = null!;

    public DbSet<FilmRoomModel> FilmRooms { get; set; } = null!;
    public DbSet<ViewerModel<FilmRoomModel>> FilmViewers { get; set; } = null!;
    public DbSet<BannedModel<FilmRoomModel>> BannedFilmViewers { get; set; } = null!;
    public DbSet<YoutubeRoomModel> YoutubeRooms { get; set; } = null!;
    public DbSet<ViewerModel<YoutubeRoomModel>> YoutubeViewers { get; set; } = null!;
    public DbSet<BannedModel<YoutubeRoomModel>> BannedYoutubeViewers { get; set; } = null!;
    public DbSet<ServerModel> Servers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureUsers();
        modelBuilder.ConfigureFilms();
        modelBuilder.ConfigurePlaylists();
        modelBuilder.ConfigureComments();
        modelBuilder.ConfigureRatings();
        modelBuilder.ConfigureFilmRooms();
        modelBuilder.ConfigureYoutubeRooms();
        base.OnModelCreating(modelBuilder);
    }
}