using MediatR;
using Microsoft.EntityFrameworkCore;
using Overoom.Infrastructure.Storage.Models.Comment;
using Overoom.Infrastructure.Storage.Models.Country;
using Overoom.Infrastructure.Storage.Models.Film;
using Overoom.Infrastructure.Storage.Models.FilmRoom;
using Overoom.Infrastructure.Storage.Models.Genre;
using Overoom.Infrastructure.Storage.Models.Person;
using Overoom.Infrastructure.Storage.Models.Playlist;
using Overoom.Infrastructure.Storage.Models.Rating;
using Overoom.Infrastructure.Storage.Models.Room.YoutubeRoom;
using Overoom.Infrastructure.Storage.Models.User;
using Overoom.Infrastructure.Storage.Models.Voice;
using Overoom.Infrastructure.Storage.Models.YoutubeRoom;

namespace Overoom.Infrastructure.Storage.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    internal List<INotification> Notifications { get; } = new();
    internal DbSet<UserModel> Users { get; set; } = null!;
    internal DbSet<HistoryModel> UserHistory { get; set; } = null!;
    internal DbSet<WatchlistModel> UserWatchlist { get; set; } = null!;


    internal DbSet<FilmModel> Films { get; set; } = null!;
    internal DbSet<CdnModel> FilmCdns { get; set; } = null!;
    internal DbSet<FilmActorModel> FilmActors { get; set; } = null!;


    internal DbSet<PersonModel> Persons { get; set; } = null!;
    internal DbSet<CountryModel> Countries { get; set; } = null!;
    internal DbSet<GenreModel> Genres { get; set; } = null!;
    internal DbSet<VoiceModel> Voices { get; set; } = null!;


    internal DbSet<PlaylistModel> Playlists { get; set; } = null!;
    internal DbSet<PlaylistFilmModel> PlaylistFilms { get; set; } = null!;

    internal DbSet<CommentModel> Comments { get; set; } = null!;

    internal DbSet<RatingModel> Ratings { get; set; } = null!;

    internal DbSet<FilmRoomModel> FilmRooms { get; set; } = null!;
    internal DbSet<FilmViewerModel> FilmRoomViewers { get; set; } = null!;
    internal DbSet<FilmMessageModel> FilmRoomMessages { get; set; } = null!;
    internal DbSet<YoutubeRoomModel> YoutubeRooms { get; set; } = null!;
    internal DbSet<YoutubeViewerModel> YoutubeRoomViewers { get; set; } = null!;
    internal DbSet<YoutubeMessageModel> YoutubeRoomMessages { get; set; } = null!;
    internal DbSet<VideoIdModel> YoutubeRoomVideoIds { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>().HasMany(x => x.History).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserModel>().HasMany(x => x.Watchlist).WithOne(x => x.User)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserModel>().HasMany(x => x.Genres).WithMany().UsingEntity(e => e.ToTable("UserGenres"));

        modelBuilder.Entity<WatchlistModel>().HasOne(x => x.Film).WithMany().HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<HistoryModel>().HasOne(x => x.Film).WithMany().HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<FilmRoomModel>().HasMany(x => x.Messages).WithOne(x => x.Room).HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<FilmRoomModel>().HasMany(x => x.Viewers).WithOne(x => x.Room).HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<FilmRoomModel>().HasOne(x => x.Film).WithMany().HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<FilmMessageModel>().HasOne(x => x.Viewer).WithMany().HasForeignKey(x => x.ViewerId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<YoutubeRoomModel>().HasMany(x => x.Messages).WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<YoutubeRoomModel>().HasMany(x => x.Viewers).WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<YoutubeMessageModel>().HasOne(x => x.Viewer).WithMany().HasForeignKey(x => x.ViewerId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<YoutubeRoomModel>().HasMany(x => x.VideoIds).WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<FilmModel>().HasMany(x => x.Actors).WithOne(x => x.Film).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<FilmModel>().HasMany(x => x.Directors).WithMany()
            .UsingEntity(e => e.ToTable("FilmDirectors"));
        modelBuilder.Entity<FilmModel>().HasMany(x => x.ScreenWriters).WithMany()
            .UsingEntity(e => e.ToTable("FilmScreenWriters"));
        modelBuilder.Entity<FilmModel>().HasMany(x => x.Genres).WithMany().UsingEntity(e => e.ToTable("FilmGenres"));
        modelBuilder.Entity<FilmModel>().HasMany(x => x.Countries).WithMany()
            .UsingEntity(e => e.ToTable("FilmCountries"));
        modelBuilder.Entity<FilmModel>().HasMany(x => x.CdnList).WithOne(x => x.Film)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<CdnModel>().HasMany(x => x.Voices).WithMany().UsingEntity(e => e.ToTable("CndVoices"));
        modelBuilder.Entity<FilmActorModel>().HasOne(x => x.Person).WithMany().OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlaylistModel>().HasMany(x => x.Films).WithOne(x => x.Playlist)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<PlaylistModel>().HasMany(x => x.Genres).WithMany()
            .UsingEntity(e => e.ToTable("PlaylistGenres"));
        modelBuilder.Entity<PlaylistFilmModel>().HasOne(x => x.Film).WithMany().HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RatingModel>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<RatingModel>().HasOne(x => x.Film).WithMany().HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CommentModel>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<CommentModel>().HasOne(x => x.Film).WithMany().HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);
        base.OnModelCreating(modelBuilder);
    }
}