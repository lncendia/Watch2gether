using Overoom.Application.Abstractions.Film.Load.Interfaces;
using Overoom.Application.Abstractions.User.Interfaces;
using Overoom.Infrastructure.Mailing;
using Overoom.Infrastructure.Movie;
using Overoom.Infrastructure.PhotoManager;

namespace Overoom.Extensions;

public static class InfrastructureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, string rootPath)
    {
        services.AddScoped<IEmailService, EmailService>(_ =>
            new EmailService("egor.lazeba@yandex.ru", "ilizzhetwisfwirw", "smtp.yandex.ru", 587));

        services.AddScoped<IUserThumbnailService, UserThumbnailService>(
            _ => new UserThumbnailService(Path.Combine(rootPath, "img")));
        services.AddScoped<IFilmPosterService, FilmPosterService>(
            _ => new FilmPosterService(Path.Combine(rootPath, "img")));

        services.AddScoped<IFilmInfoService, FilmService>(_ =>
            new FilmService("6oDZugvTXZogUnTodylqzeEP7c4lmnkd", "e2f56e43-04aa-4388-8852-addef6f31247"));
    }
}