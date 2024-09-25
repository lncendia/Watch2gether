using Films.Infrastructure.Storage.Models.Comments;
using Films.Infrastructure.Storage.Models.FilmRooms;
using Films.Infrastructure.Storage.Models.Films;
using Films.Infrastructure.Storage.Models.Playlists;
using Films.Infrastructure.Storage.Models.Ratings;
using Films.Infrastructure.Storage.Models.Rooms;
using Films.Infrastructure.Storage.Models.Servers;
using Films.Infrastructure.Storage.Models.Users;
using Films.Infrastructure.Storage.Models.YoutubeRoom;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Context;

public static class FluentExtensions
{
    public static void ConfigureFilms(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FilmModel>()
            .HasMany(x => x.Actors)
            .WithOne(x => x.Film)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FilmModel>()
            .HasMany(x => x.Directors)
            .WithMany()
            .UsingEntity(e => e.ToTable("FilmDirectors"));

        modelBuilder.Entity<FilmModel>()
            .HasMany(x => x.Screenwriters)
            .WithMany()
            .UsingEntity(e => e.ToTable("FilmScreenWriters"));

        modelBuilder.Entity<FilmModel>()
            .HasMany(x => x.Genres)
            .WithMany()
            .UsingEntity(e => e.ToTable("FilmGenres"));

        modelBuilder.Entity<FilmModel>()
            .HasMany(x => x.Countries)
            .WithMany()
            .UsingEntity(e => e.ToTable("FilmCountries"));

        modelBuilder.Entity<FilmModel>()
            .HasMany(x => x.CdnList)
            .WithOne(x => x.Film)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FilmActorModel>()
            .HasOne(x => x.Person)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FilmModel>()
            .HasMany(x => x.Playlists)
            .WithOne()
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public static void ConfigureUsers(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>()
            .HasMany(x => x.History)
            .WithOne(x => x.User)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserModel>()
            .HasMany(x => x.Watchlist)
            .WithOne(x => x.User)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserModel>()
            .HasMany(x => x.Genres)
            .WithMany()
            .UsingEntity(e => e.ToTable("UserGenres"));

        modelBuilder.Entity<WatchlistModel>()
            .HasOne(x => x.Film)
            .WithMany()
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HistoryModel>()
            .HasOne(x => x.Film)
            .WithMany()
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public static void ConfigurePlaylists(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlaylistModel>()
            .HasMany(x => x.Films)
            .WithOne()
            .HasForeignKey(p => p.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlaylistModel>()
            .HasMany(x => x.Genres)
            .WithMany()
            .UsingEntity(e => e.ToTable("PlaylistGenres"));
    }

    public static void ConfigureRatings(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RatingModel>()
            .HasOne<UserModel>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<RatingModel>()
            .HasOne<FilmModel>()
            .WithMany(x=>x.Ratings)
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public static void ConfigureComments(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommentModel>()
            .HasOne<UserModel>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<CommentModel>()
            .HasOne<FilmModel>()
            .WithMany()
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public static void ConfigureFilmRooms(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FilmRoomModel>()
            .HasOne<FilmModel>()
            .WithMany()
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FilmRoomModel>()
            .HasOne<ServerModel>()
            .WithMany(x=>x.FilmRooms)
            .HasForeignKey(x => x.ServerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FilmRoomModel>()
            .HasOne<ServerModel>()
            .WithOne()
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FilmRoomModel>()
            .HasMany(x => x.Banned)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ViewerModel<FilmRoomModel>>()
            .HasOne<UserModel>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BannedModel<FilmRoomModel>>()
            .HasOne<UserModel>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public static void ConfigureYoutubeRooms(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<YoutubeRoomModel>()
            .HasMany(x => x.Viewers)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<YoutubeRoomModel>()
            .HasMany(x => x.Banned)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<YoutubeRoomModel>()
            .HasOne<ServerModel>()
            .WithMany(x=>x.YoutubeRooms)
            .HasForeignKey(x => x.ServerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ViewerModel<YoutubeRoomModel>>()
            .HasOne<UserModel>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BannedModel<YoutubeRoomModel>>()
            .HasOne<UserModel>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}