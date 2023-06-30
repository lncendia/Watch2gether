using MediatR;
using Microsoft.EntityFrameworkCore;
using Overoom.Infrastructure.Storage.Models.Comment;
using Overoom.Infrastructure.Storage.Models.Film;
using Overoom.Infrastructure.Storage.Models.Playlist;
using Overoom.Infrastructure.Storage.Models.Rating;
using Overoom.Infrastructure.Storage.Models.Room.Base;
using Overoom.Infrastructure.Storage.Models.Room.FilmRoom;
using Overoom.Infrastructure.Storage.Models.Room.YoutubeRoom;
using Overoom.Infrastructure.Storage.Models.User;

namespace Overoom.Infrastructure.Storage.Context;

public class ApplicationDbContext : DbContext
{
    internal ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    internal List<INotification> Notifications { get; } = new();
    internal DbSet<UserModel> Users { get; set; } = null!;
    internal DbSet<HistoryModel> UserHistory { get; set; } = null!;
    internal DbSet<WatchlistModel> UserWatchlist { get; set; } = null!;


    internal DbSet<FilmModel> Films { get; set; } = null!;
    internal DbSet<ActorModel> FilmActors { get; set; } = null!;
    internal DbSet<DirectorModel> FilmDirectors { get; set; } = null!;
    internal DbSet<ScreenWriterModel> FilmScreenWriters { get; set; } = null!;
    internal DbSet<GenreModel> FilmGenres { get; set; } = null!;
    internal DbSet<CountryModel> FilmCountries { get; set; } = null!;
    internal DbSet<CdnModel> FilmCdn { get; set; } = null!;
    internal DbSet<VoiceModel> CdnVoices { get; set; } = null!;


    internal DbSet<PlaylistModel> Playlists { get; set; } = null!;
    internal DbSet<PlaylistFilmModel> PlaylistFilms { get; set; } = null!;
    internal DbSet<PlaylistGenreModel> PlaylistGenres { get; set; } = null!;

    internal DbSet<CommentModel> Comments { get; set; } = null!;

    internal DbSet<RatingModel> Ratings { get; set; } = null!;

    internal DbSet<FilmRoomModel> FilmRooms { get; set; } = null!;
    internal DbSet<FilmViewerModel> FilmViewers { get; set; } = null!;
    internal DbSet<YoutubeRoomModel> YoutubeRooms { get; set; } = null!;
    internal DbSet<YoutubeViewerModel> YoutubeViewers { get; set; } = null!;
    internal DbSet<VideoIdModel> VideoIds { get; set; } = null!;


    internal DbSet<MessageModel> Messages { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>().HasMany(x => x.History).WithOne().OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserModel>().HasMany(x => x.Watchlist).WithOne().OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WatchlistModel>().HasOne(x => x.Film).WithMany().HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<HistoryModel>().HasOne(x => x.Film).WithMany().HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<RoomModel>().HasMany(x => x.Messages).WithOne(x => x.Room).HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<RoomModel>().HasMany(x => x.Viewers).WithOne(x => x.Room).HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<MessageModel>().HasOne(x => x.Viewer).WithMany().HasForeignKey(x => x.ViewerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<YoutubeRoomModel>().HasMany(x => x.VideoIds).WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<FilmModel>().HasMany(x => x.Actors).WithOne(x => x.FilmModel)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<FilmModel>().HasMany(x => x.Directors).WithOne(x => x.FilmModel)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<FilmModel>().HasMany(x => x.ScreenWriters).WithOne(x => x.FilmModel)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<FilmModel>().HasMany(x => x.Genres).WithOne(x => x.FilmModel)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<FilmModel>().HasMany(x => x.Countries).WithOne(x => x.FilmModel)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<FilmModel>().HasMany(x => x.CdnList).WithOne().OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<CdnModel>().HasMany(x => x.Voices).WithOne(x => x.Cdn).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlaylistModel>().HasMany(x => x.Films).WithOne().OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<PlaylistFilmModel>().HasOne(x => x.Film).WithMany().HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<PlaylistGenreModel>().HasOne(x => x.PlaylistModel).WithMany(x => x.Genres)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RatingModel>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<RatingModel>().HasOne(x => x.Film).WithMany().HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CommentModel>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<CommentModel>().HasOne(x => x.Film).WithMany().HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<RoomModel>().UseTpcMappingStrategy();
        modelBuilder.Entity<ViewerModel>().UseTpcMappingStrategy();
    }
}