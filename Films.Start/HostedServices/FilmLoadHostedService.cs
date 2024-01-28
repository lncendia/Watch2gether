using Films.Start.Application.Abstractions.FilmsLoading;
using Films.Start.Application.Abstractions.Movie.Exceptions;
using Films.Start.Application.Abstractions.MovieApi.Exceptions;
using Films.Start.Domain.Films.Exceptions;

namespace Films.Start.HostedServices;

public class FilmLoadHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<FilmLoadHostedService> _logger;

    public FilmLoadHostedService(IServiceProvider serviceProvider, ILogger<FilmLoadHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var list = new List<long>
        {
        };
        foreach (var id in list)
        {
            await Task.Delay(1500, stoppingToken);
            try
            {
                await AddFilmAsync(id, stoppingToken);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case ApiNotFoundException:
                        _logger.LogWarning("Information not found for ID {ID}", id);
                        break;
                    case EmptyCdnsCollectionException:
                        _logger.LogWarning("CDNs not found for ID {ID}", id);
                        break;
                    case FilmAlreadyExistsException:
                        _logger.LogWarning("Film {ID} already exists", id);
                        break;
                    default:
                        throw;
                }
            }
        }

        _logger.LogInformation("All films are uploaded");
    }

    private async Task AddFilmAsync(long id, CancellationToken token)
    {
        using var scope = _serviceProvider.CreateScope();
        var autoLoader = scope.ServiceProvider.GetRequiredService<IFilmAutoLoader>();
        await autoLoader.LoadAsync(id, token);
    }
}