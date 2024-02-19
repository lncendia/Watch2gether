using Films.Application.Abstractions.Commands.FilmsManagement;
using Films.Application.Abstractions.Queries.FilmsApi;
using MediatR;

namespace Films.Start.HostedServices;

public class FilmLoadHostedService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int[] films = [447301];
        foreach (var film in films)
        {
            using var scope = serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var filmFromKp = await mediator.Send(new FindFilmByKpIdQuery { Id = film }, stoppingToken);
            await mediator.Send(new AddFilmCommand
            {
                Title = filmFromKp.Title,
                Description = filmFromKp.Description!,
                ShortDescription = filmFromKp.ShortDescription,
                IsSerial = filmFromKp.IsSerial,
                Year = filmFromKp.Year,
                CdnList = filmFromKp.Cdn,
                Countries = filmFromKp.Countries,
                Actors = filmFromKp.Actors.Take(15).ToArray(),
                Directors = filmFromKp.Directors,
                Genres = filmFromKp.Genres,
                Screenwriters = filmFromKp.Screenwriters,
                CountSeasons = filmFromKp.CountSeasons,
                CountEpisodes = filmFromKp.CountEpisodes,
                RatingKp = filmFromKp.RatingKp,
                RatingImdb = filmFromKp.RatingImdb,
                PosterUrl = filmFromKp.PosterUrl
            }, stoppingToken);
        }
    }
}