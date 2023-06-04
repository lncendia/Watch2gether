using Microsoft.EntityFrameworkCore;
using Overoom.Infrastructure.Storage.Models.Comments;
using Overoom.Infrastructure.Storage.Models.Films;
using Overoom.Infrastructure.Storage.Models.Playlists;
using Overoom.Infrastructure.Storage.Models.Rooms;
using Overoom.Infrastructure.Storage.Models.Rooms.Base;
using Overoom.Infrastructure.Storage.Models.Users;

namespace Overoom.Infrastructure.Storage.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserModel> Users { get; set; } = null!;


    public DbSet<FilmModel> Films { get; set; } = null!;
    public DbSet<ActorModel> Actors { get; set; } = null!;
    public DbSet<DirectorModel> Directors { get; set; } = null!;
    public DbSet<ScreenWriterModel> ScreenWriters { get; set; } = null!;
    public DbSet<GenreModel> Genres { get; set; } = null!;
    public DbSet<CountryModel> Countries { get; set; } = null!;


    public DbSet<PlaylistModel> Playlists { get; set; } = null!;

    public DbSet<CommentModel> Comments { get; set; } = null!;


    public DbSet<FilmRoomModel> FilmRooms { get; set; } = null!;
    public DbSet<YoutubeRoomModel> YoutubeRooms { get; set; } = null!;
    public DbSet<VideoIdModel> VideoIds { get; set; } = null!;
    public DbSet<MessageModel> Messages { get; set; } = null!;

    public DbSet<YoutubeViewerModel> YoutubeViewers { get; set; } = null!;
    public DbSet<FilmViewerModel> FilmViewers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoomBaseModel>().HasMany(x => x.Messages).WithOne(x => x.Room).HasForeignKey(x => x.RoomId);

        modelBuilder.Entity<FilmRoomModel>().HasMany(x => x.Viewers).WithOne(x => (FilmRoomModel) x.Room)
            .HasForeignKey(x => x.RoomId);


        modelBuilder.Entity<YoutubeRoomModel>().HasMany(x => x.Viewers).WithOne(x => (YoutubeRoomModel) x.Room)
            .HasForeignKey(x => x.RoomId);
        modelBuilder.Entity<YoutubeRoomModel>().HasMany(x => x.VideoIds).WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId);

        modelBuilder.Entity<MessageModel>().HasOne(x => x.Viewer).WithMany().HasForeignKey(x => x.ViewerId);

        modelBuilder.Entity<FilmModel>().HasMany(x => x.Actors).WithOne(x => x.FilmModel);
        modelBuilder.Entity<FilmModel>().HasMany(x => x.Directors).WithOne(x => x.FilmModel);
        modelBuilder.Entity<FilmModel>().HasMany(x => x.ScreenWriters).WithOne(x => x.FilmModel);
        modelBuilder.Entity<FilmModel>().HasMany(x => x.Genres).WithOne(x => x.FilmModel);
        modelBuilder.Entity<FilmModel>().HasMany(x => x.Countries).WithOne(x => x.FilmModel);
    }
}