using Watch2gether.Application.Abstractions.Interfaces.Films;
using Watch2gether.Application.Abstractions.Interfaces.Users;
using Watch2gether.Infrastructure.Mailing;
using Watch2gether.Infrastructure.MovieDownloader;
using Watch2gether.Infrastructure.PhotoManager;

namespace Watch2gether.Extensions;

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

        services.AddScoped<IFilmInfoGetterService, FilmGetterService>(_ =>
            new FilmGetterService("6oDZugvTXZogUnTodylqzeEP7c4lmnkd", "e2f56e43-04aa-4388-8852-addef6f31247"));
    }
}