using MediatR;
using Microsoft.EntityFrameworkCore;
using Room.Infrastructure.Storage.Models.FilmRooms;
using Room.Infrastructure.Storage.Models.Messages;
using Room.Infrastructure.Storage.Models.YoutubeRooms;

namespace Room.Infrastructure.Storage.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public List<INotification> Notifications { get; } = [];


    public DbSet<FilmRoomModel> FilmRooms { get; set; } = null!;

    public DbSet<FilmViewerModel> FilmViewers { get; set; } = null!;

    public DbSet<MessageModel<FilmRoomModel>> FilmMessages { get; set; } = null!;


    public DbSet<YoutubeRoomModel> YoutubeRooms { get; set; } = null!;

    public DbSet<YoutubeViewerModel> YoutubeViewers { get; set; } = null!;

    public DbSet<MessageModel<YoutubeRoomModel>> YoutubeMessages { get; set; } = null!;

    public DbSet<VideoModel> YoutubeVideoIds { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FilmRoomModel>()
            .HasMany(x => x.Viewers)
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

        modelBuilder.Entity<MessageModel<FilmRoomModel>>()
            .HasOne(m => m.Room)
            .WithMany()
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<MessageModel<YoutubeRoomModel>>()
            .HasOne(m => m.Room)
            .WithMany()
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}