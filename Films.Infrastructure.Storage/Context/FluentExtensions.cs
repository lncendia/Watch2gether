using Films.Infrastructure.Storage.Models.Comment;
using Films.Infrastructure.Storage.Models.Film;
using Films.Infrastructure.Storage.Models.Playlist;
using Films.Infrastructure.Storage.Models.Rating;
using Films.Infrastructure.Storage.Models.Rooms.BaseRoom;
using Films.Infrastructure.Storage.Models.Rooms.FilmRoom;
using Films.Infrastructure.Storage.Models.Rooms.YoutubeRoom;
using Films.Infrastructure.Storage.Models.User;
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

        modelBuilder.Entity<CdnModel>()
            .HasMany(x => x.Voices)
            .WithMany()
            .UsingEntity(e => e.ToTable("CndVoices"));

        modelBuilder.Entity<FilmActorModel>()
            .HasOne(x => x.Person)
            .WithMany()
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
            .WithOne(x => x.Playlist)
            .HasForeignKey(p => p.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlaylistFilmModel>()
            .HasOne(x => x.Film)
            .WithMany()
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlaylistModel>()
            .HasMany(x => x.Genres)
            .WithMany()
            .UsingEntity(e => e.ToTable("PlaylistGenres"));
    }
    
    public static void ConfigureRatings(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RatingModel>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<RatingModel>()
            .HasOne(x => x.Film)
            .WithMany()
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    public static void ConfigureComments(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommentModel>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<CommentModel>()
            .HasOne(x => x.Film)
            .WithMany()
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    public static void ConfigureFilmRooms(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FilmRoomModel>()
            .HasOne(x => x.Film)
            .WithMany()
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<FilmRoomModel>()
            .HasOne(x => x.Server)
            .WithMany()
            .HasForeignKey(x => x.ServerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<FilmRoomModel>()
            .HasMany(x => x.Viewers)
            .WithOne(x=>x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<FilmRoomModel>()
            .HasMany(x => x.Banned)
            .WithOne(x=>x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ViewerModel<FilmRoomModel>>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<BannedModel<FilmRoomModel>>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    public static void ConfigureYoutubeRooms(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<YoutubeRoomModel>()
            .HasMany(x => x.Viewers)
            .WithOne(x=>x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<YoutubeRoomModel>()
            .HasMany(x => x.Banned)
            .WithOne(x=>x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<YoutubeRoomModel>()
            .HasOne(x => x.Server)
            .WithMany()
            .HasForeignKey(x => x.ServerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ViewerModel<YoutubeRoomModel>>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<BannedModel<YoutubeRoomModel>>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}