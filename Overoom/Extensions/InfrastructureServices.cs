using Overoom.Application.Abstractions.Films.Kinopoisk.Interfaces;
using Overoom.Application.Abstractions.Films.Load.Interfaces;
using Overoom.Application.Abstractions.Users.Interfaces;
using Overoom.Infrastructure.Mailing;
using Overoom.Infrastructure.Movie;
using Overoom.Infrastructure.Movie.Abstractions;
using Overoom.Infrastructure.PhotoManager;

namespace Overoom.Extensions;

public static class InfrastructureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, string rootPath)
    {
        services.AddScoped<IEmailService, EmailService>(_ =>
            new EmailService("egor.lazeba@yandex.ru", "ilizzhetwisfwirw", "smtp.yandex.ru", 587));

        services.AddScoped<IUserThumbnailService, UserThumbnailService>(
            _ => new UserThumbnailService(rootPath, "img/avatars"));
        services.AddScoped<IFilmPosterService, FilmPosterService>(
            _ => new FilmPosterService(Path.Combine(rootPath, "img/posters")));


        services.AddScoped<IResponseParser, ResponseParser>();
        services.AddScoped<IKpApiService, KpApi>(s => new KpApi("dd", s.GetRequiredService<IResponseParser>()));
    }
}