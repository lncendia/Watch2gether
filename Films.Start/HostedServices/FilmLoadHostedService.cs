using Films.Application.Abstractions.Commands.FilmsManagement;
using Films.Application.Abstractions.Queries.FilmsApi;
using Films.Domain.Films.Enums;
using Films.Domain.Films.ValueObjects;
using Films.Infrastructure.Storage.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Films.Start.HostedServices;

public class FilmLoadHostedService(IServiceProvider serviceProvider) : BackgroundService
{
    // protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    // {
    //     int[] films = [447301];
    //     foreach (var film in films)
    //     {
    //         using var scope = serviceProvider.CreateScope();
    //         var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    //         var filmFromKp = await mediator.Send(new FindFilmByKpIdQuery { Id = film }, stoppingToken);
    //         await mediator.Send(new AddFilmCommand
    //         {
    //             Title = filmFromKp.Title,
    //             Description = filmFromKp.Description!,
    //             ShortDescription = filmFromKp.ShortDescription,
    //             Type = filmFromKp.Type,
    //             Year = filmFromKp.Year,
    //             CdnList = filmFromKp.Cdn,
    //             Countries = filmFromKp.Countries,
    //             Actors = filmFromKp.Actors.Take(15).ToArray(),
    //             Directors = filmFromKp.Directors,
    //             Genres = filmFromKp.Genres,
    //             Screenwriters = filmFromKp.Screenwriters,
    //             CountSeasons = filmFromKp.CountSeasons,
    //             CountEpisodes = filmFromKp.CountEpisodes,
    //             RatingKp = filmFromKp.RatingKp,
    //             RatingImdb = filmFromKp.RatingImdb,
    //             PosterUrl = filmFromKp.PosterUrl
    //         }, stoppingToken);
    //     }
    // }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext2>();

        var films = await context.Films
            .Include(filmModel => filmModel.Countries)
            .Include(filmModel => filmModel.Actors)
            .ThenInclude(model => model.Person)
            .Include(filmModel => filmModel.Directors)
            .Include(filmModel => filmModel.Genres)
            .Include(filmModel => filmModel.Screenwriters)
            .Include(filmModel => filmModel.CdnList)
            .ThenInclude(cdnModel => cdnModel.Voices)
            .ToArrayAsync(cancellationToken: stoppingToken);

        foreach (var film in films)
        {
            using var dscope = scope.ServiceProvider.CreateScope();
            var mediator = dscope.ServiceProvider.GetRequiredService<IMediator>();
            if (film.CdnList.All(c => c.Type != CdnType.VideoCdn)) continue;
            await mediator.Send(new AddFilmCommand
            {
                Title = film.Title,
                Description = film.Description,
                ShortDescription = film.ShortDescription,
                Type = film.Type,
                Year = film.Year,
                CdnList = film.CdnList.Where(x => x.Type == CdnType.VideoCdn).Select(x => new Cdn
                {
                    Quality = x.Quality, Type = x.Type,
                    Url = x.Url, Voices = x.Voices.Select(a => a.Name).ToArray()
                }).ToArray(),
                Countries = film.Countries.Select(a => a.Name).ToArray(),
                Actors = film.Actors.Take(15).Select(a => new Actor(a.Person.Name, a.Description)).ToArray(),
                Directors = film.Directors.Select(a => a.Name).ToArray(),
                Genres = film.Genres.Select(a => a.Name).ToArray(),
                Screenwriters = film.Screenwriters.Select(a => a.Name).ToArray(),
                CountSeasons = film.CountSeasons,
                CountEpisodes = film.CountEpisodes,
                RatingKp = film.RatingKp,
                RatingImdb = film.RatingKp,
                PosterUrl = film.PosterUrl
            }, stoppingToken);
        }
    }
}