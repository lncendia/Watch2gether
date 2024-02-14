using MediatR;
using Microsoft.EntityFrameworkCore;
using Room.Infrastructure.Storage.Models.Film;
using Room.Infrastructure.Storage.Models.Room.Base;
using Room.Infrastructure.Storage.Models.Room.FilmRoom;
using Room.Infrastructure.Storage.Models.Room.YoutubeRoom;
using Room.Infrastructure.Storage.Models.User;

namespace Room.Infrastructure.Storage.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public List<INotification> Notifications { get; } = [];
    public DbSet<UserModel> Users { get; set; } = null!;
    public DbSet<FilmModel> Films { get; set; } = null!;
    
    public DbSet<CdnModel> FilmCdns { get; set; } = null!;

    
    public DbSet<FilmRoomModel> FilmRooms { get; set; } = null!;
    
    public DbSet<FilmViewerModel> FilmViewers { get; set; } = null!;
    
    public DbSet<MessageModel<FilmRoomModel>> FilmViewersMessages { get; set; } = null!;
    
    public DbSet<BannedModel<FilmRoomModel>> FilmRoomsBannedUsers { get; set; }= null!;
    
    
    
    public DbSet<YoutubeRoomModel> YoutubeRooms { get; set; } = null!;
    
    public DbSet<YoutubeViewerModel> YoutubeViewers { get; set; } = null!;
    
    public DbSet<MessageModel<YoutubeRoomModel>> YoutubeViewersMessages { get; set; } = null!;
    
    public DbSet<BannedModel<YoutubeRoomModel>> YoutubeRoomsBannedUsers { get; set; }= null!;
    
    public DbSet<VideoModel> YoutubeRoomsVideoIds { get; set; }= null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FilmModel>()
            .HasMany(x => x.CdnList)
            .WithOne(x => x.Film)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<FilmRoomModel>()
            .HasMany(x => x.Viewers)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<FilmRoomModel>()
            .HasMany(x => x.BannedUsers)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<FilmRoomModel>()
            .HasOne(x => x.Film)
            .WithMany()
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<FilmRoomModel>()
            .HasMany(x => x.Messages)
            .WithOne(x=>x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<FilmViewerModel>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<BannedModel<FilmRoomModel>>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<MessageModel<FilmRoomModel>>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
        
        modelBuilder.Entity<YoutubeRoomModel>()
            .HasMany(x => x.Viewers)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<YoutubeRoomModel>()
            .HasMany(x => x.BannedUsers)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<YoutubeRoomModel>()
            .HasMany(x => x.Videos)
            .WithOne(x=>x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<YoutubeRoomModel>()
            .HasMany(x => x.Messages)
            .WithOne(x=>x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<YoutubeViewerModel>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<BannedModel<YoutubeRoomModel>>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MessageModel<YoutubeRoomModel>>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
}