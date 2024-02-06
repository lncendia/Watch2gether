using Films.Infrastructure.Storage.Models.Country;
using Films.Infrastructure.Storage.Models.Film;
using Films.Infrastructure.Storage.Models.Genre;
using Films.Infrastructure.Storage.Models.Person;
using Films.Infrastructure.Storage.Models.Voice;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Context;

public class ApplicationDbContext2(DbContextOptions<ApplicationDbContext2> options) : DbContext(options)
{
    public DbSet<FilmModel> Films { get; set; } = null!;
    public DbSet<CdnModel> FilmCdns { get; set; } = null!;
    public DbSet<FilmActorModel> FilmActors { get; set; } = null!;
    
    public DbSet<PersonModel> Persons { get; set; } = null!;
    public DbSet<CountryModel> Countries { get; set; } = null!;
    public DbSet<GenreModel> Genres { get; set; } = null!;
    public DbSet<VoiceModel> Voices { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
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
        
        base.OnModelCreating(modelBuilder);
    }
}