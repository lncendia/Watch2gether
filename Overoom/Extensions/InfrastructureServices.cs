using Overoom.Application.Abstractions.Common.Interfaces;
using Overoom.Application.Abstractions.Kinopoisk.Interfaces;
using Overoom.Infrastructure.Mailing;
using Overoom.Infrastructure.Movie.Abstractions;
using Overoom.Infrastructure.Movie.Services;
using Overoom.Infrastructure.PhotoManager;

namespace Overoom.Extensions;

public static class InfrastructureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, string rootPath)
    {
        services.AddScoped<IEmailService, EmailService>(_ =>
            new EmailService("egor.lazeba@yandex.ru", "ilizzhetwisfwirw", "smtp.yandex.ru", 587));

        services.AddScoped<IUserThumbnailService, UserThumbnailService>(
            _ => new UserThumbnailService(rootPath, Path.Combine("img", "avatars")));
        services.AddScoped<IPosterService, PosterService>(
            _ => new PosterService(rootPath, Path.Combine("img", "posters")));


        services.AddScoped<IKpResponseParser, KpResponseParser>();
        services.AddScoped<IVideoCdnResponseParser, VideoCdnResponseParser>();
        services.AddScoped<IKpApiService, KpApi>(s =>
            new KpApi("e2f56e43-04aa-4388-8852-addef6f31247", s.GetRequiredService<IKpResponseParser>()));
        services.AddScoped<IVideoCdnApiService, VideoCdnApi>(s =>
            new VideoCdnApi("6oDZugvTXZogUnTodylqzeEP7c4lmnkd", s.GetRequiredService<IVideoCdnResponseParser>()));
    }
}