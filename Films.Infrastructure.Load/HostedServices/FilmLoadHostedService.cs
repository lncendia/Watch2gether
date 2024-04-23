using Films.Application.Abstractions.Commands.FilmsManagement;
using Films.Domain.Films.ValueObjects;
using Films.Infrastructure.Load.Kinopoisk;
using Films.Infrastructure.Load.Kinopoisk.Enums;
using Films.Infrastructure.Load.Kodik;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Films.Infrastructure.Load.HostedServices;

public class FilmLoadHostedService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var kodik = scope.ServiceProvider.GetRequiredService<IKodikApi>();
        var kpApi = scope.ServiceProvider.GetRequiredService<IKinopoiskApi>();

        var ids = new[] { 1293403 };

        foreach (var id in ids)
        {
            var filmKp = await kpApi.GetAsync(id, stoppingToken);

            var persons = await kpApi.GetActorsAsync(id, stoppingToken);

            var filmData = await kodik.GetByKinopoiskIdAsync(id, true, stoppingToken);

            var film = filmData.FirstOrDefault();

            if (film == null) continue;

            await mediator.Send(new AddFilmCommand
            {
                CdnList = new[]
                {
                    new Cdn
                    {
                        Name = "Kodik",
                        Quality = film.Quality,
                        Url = new Uri($"http:{film.Link}")
                    }
                },
                Description = film.MaterialData.Description,
                ShortDescription = filmKp.ShortDescription,
                IsSerial = filmKp.Serial,
                CountEpisodes = film.EpisodesCount,
                CountSeasons = film.LastSeason,
                Title = film.Title,
                Year = film.Year,
                PosterUrl = new Uri(film.MaterialData.PosterUrl),
                Countries = film.MaterialData.Countries,
                Actors = persons
                    .Where(p => p.Profession == Profession.Actor)
                    .Select(p => new Actor(GetName(p.Name, p.NameEn)!, p.Description))
                    .ToArray(),
                Directors = persons
                    .Where(p => p.Profession == Profession.Director)
                    .Select(p => GetName(p.Name, p.NameEn)!)
                    .ToArray(),
                Genres = film.MaterialData.Genres,
                Screenwriters = persons
                    .Where(p => p.Profession == Profession.Writer)
                    .Select(p => GetName(p.Name, p.NameEn)!)
                    .ToArray(),
            }, stoppingToken);

            await Task.Delay(5000, stoppingToken);
        }
    }

    private static string? GetName(string? ru, string? en)
    {
        if (!string.IsNullOrEmpty(ru)) return ru;
        if (!string.IsNullOrEmpty(en)) return en;
        return null;
    }
}