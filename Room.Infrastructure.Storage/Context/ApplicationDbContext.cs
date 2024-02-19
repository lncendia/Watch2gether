using MediatR;
using Microsoft.EntityFrameworkCore;
using Room.Infrastructure.Storage.Models.BaseRoom;
using Room.Infrastructure.Storage.Models.FilmRoom;
using Room.Infrastructure.Storage.Models.YoutubeRoom;

namespace Room.Infrastructure.Storage.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public List<INotification> Notifications { get; } = [];


    public DbSet<FilmRoomModel> FilmRooms { get; set; } = null!;

    public DbSet<FilmViewerModel> FilmViewers { get; set; } = null!;

    public DbSet<MessageModel<FilmRoomModel>> FilmViewersMessages { get; set; } = null!;
    

    public DbSet<YoutubeRoomModel> YoutubeRooms { get; set; } = null!;

    public DbSet<YoutubeViewerModel> YoutubeViewers { get; set; } = null!;

    public DbSet<MessageModel<YoutubeRoomModel>> YoutubeViewersMessages { get; set; } = null!;

    public DbSet<VideoModel> YoutubeRoomsVideoIds { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FilmRoomModel>()
            .HasMany(x => x.Viewers)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FilmRoomModel>()
            .HasMany(x => x.Messages)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<YoutubeRoomModel>()
            .HasMany(x => x.Viewers)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<YoutubeRoomModel>()
            .HasMany(x => x.Videos)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<YoutubeRoomModel>()
            .HasMany(x => x.Messages)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}