using Films.Infrastructure.Storage.Models.Comment;
using Films.Infrastructure.Storage.Models.Country;
using Films.Infrastructure.Storage.Models.Film;
using Films.Infrastructure.Storage.Models.Genre;
using Films.Infrastructure.Storage.Models.Person;
using Films.Infrastructure.Storage.Models.Playlist;
using Films.Infrastructure.Storage.Models.Rating;
using Films.Infrastructure.Storage.Models.Room;
using Films.Infrastructure.Storage.Models.Server;
using Films.Infrastructure.Storage.Models.User;
using Films.Infrastructure.Storage.Models.Voice;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public List<INotification> Notifications { get; } = [];
    public DbSet<UserModel> Users { get; set; } = null!;
    public DbSet<HistoryModel> UserHistory { get; set; } = null!;
    public DbSet<WatchlistModel> UserWatchlist { get; set; } = null!;


    public DbSet<FilmModel> Films { get; set; } = null!;
    public DbSet<CdnModel> FilmCdns { get; set; } = null!;
    public DbSet<FilmActorModel> FilmActors { get; set; } = null!;


    public DbSet<PersonModel> Persons { get; set; } = null!;
    public DbSet<CountryModel> Countries { get; set; } = null!;
    public DbSet<GenreModel> Genres { get; set; } = null!;
    public DbSet<VoiceModel> Voices { get; set; } = null!;


    public DbSet<PlaylistModel> Playlists { get; set; } = null!;
    public DbSet<PlaylistFilmModel> PlaylistFilms { get; set; } = null!;

    public DbSet<CommentModel> Comments { get; set; } = null!;

    public DbSet<RatingModel> Ratings { get; set; } = null!;

    public DbSet<RoomModel> Rooms { get; set; } = null!;
    public DbSet<ServerModel> Servers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
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
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HistoryModel>()
            .HasOne(x => x.Film)
            .WithMany()
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<FilmModel>()
            .HasMany(x => x.Actors)
            .WithOne(x => x.Film)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FilmModel>()
            .HasMany(x => x.Directors)
            .WithMany()
            .UsingEntity(e => e.ToTable("FilmDirectors"));

        modelBuilder.Entity<FilmModel>()
            .HasMany(x => x.ScreenWriters)
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

        modelBuilder.Entity<PlaylistModel>()
            .HasMany(x => x.Films)
            .WithMany()
            .UsingEntity(e => e.ToTable("PlaylistFilms"));
        
        modelBuilder.Entity<PlaylistFilmModel>()
            .HasOne(x => x.Film)
            .WithMany()
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<PlaylistModel>()
            .HasMany(x => x.Genres)
            .WithMany()
            .UsingEntity(e => e.ToTable("PlaylistGenres"));

        modelBuilder.Entity<RatingModel>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<RatingModel>()
            .HasOne(x => x.Film)
            .WithMany()
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CommentModel>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<CommentModel>()
            .HasOne(x => x.Film)
            .WithMany()
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);
        
        base.OnModelCreating(modelBuilder);
    }
}