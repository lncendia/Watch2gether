using Microsoft.EntityFrameworkCore;
using Watch2gether.Infrastructure.PersistentStorage.Models;
using Watch2gether.Infrastructure.PersistentStorage.Models.Comments;
using Watch2gether.Infrastructure.PersistentStorage.Models.Films;
using Watch2gether.Infrastructure.PersistentStorage.Models.Playlists;
using Watch2gether.Infrastructure.PersistentStorage.Models.Rooms;
using Watch2gether.Infrastructure.PersistentStorage.Models.Users;

namespace Watch2gether.Infrastructure.PersistentStorage.Context;

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
    public DbSet<ViewerModel> Viewers { get; set; } = null!;
    public DbSet<MessageModel> Messages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoomBaseModel>().HasMany(x => x.Viewers).WithOne(x => x.Room).HasForeignKey(x => x.RoomId);
        modelBuilder.Entity<RoomBaseModel>().HasMany(x => x.Messages).WithOne(x => x.Room).HasForeignKey(x => x.RoomId);
        modelBuilder.Entity<MessageModel>().HasOne(x => x.Viewer).WithMany().HasForeignKey(x => x.ViewerId);

        modelBuilder.Entity<FilmModel>().HasMany(x => x.Actors).WithOne();
        modelBuilder.Entity<FilmModel>().HasMany(x => x.Directors).WithOne();
        modelBuilder.Entity<FilmModel>().HasMany(x => x.ScreenWriters).WithOne();
        modelBuilder.Entity<FilmModel>().HasMany(x => x.Genres).WithOne();
        modelBuilder.Entity<FilmModel>().HasMany(x => x.Countries).WithOne();
    }
}