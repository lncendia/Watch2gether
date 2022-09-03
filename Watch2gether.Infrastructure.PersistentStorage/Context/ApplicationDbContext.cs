using Microsoft.EntityFrameworkCore;
using Watch2gether.Infrastructure.PersistentStorage.Models;

namespace Watch2gether.Infrastructure.PersistentStorage.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserModel> Users { get; set; } = null!;
    public DbSet<FilmModel> Films { get; set; } = null!;
    public DbSet<PlaylistModel> Playlists { get; set; } = null!;

    // public DbSet<CommentModel> Comments { get; set; } = null!;
    public DbSet<RoomModel> Rooms { get; set; } = null!;
    public DbSet<ViewerModel> Viewers { get; set; } = null!;
    public DbSet<MessageModel> Messages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoomModel>().HasMany(x => x.Viewers).WithOne(x => x.Room).HasForeignKey(x => x.RoomId);
        modelBuilder.Entity<RoomModel>().HasMany(x => x.Messages).WithOne(x=>x.Room).HasForeignKey(x => x.RoomId);
        modelBuilder.Entity<MessageModel>().HasOne(x => x.Viewer).WithMany().HasForeignKey(x => x.ViewerId);
    }
}