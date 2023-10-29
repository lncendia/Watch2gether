using Overoom.Application.Abstractions.Common.Interfaces;
using Overoom.Application.Abstractions.MovieApi.Interfaces;
using Overoom.Exceptions;
using Overoom.Infrastructure.Mailing;
using Overoom.Infrastructure.Movie.Abstractions;
using Overoom.Infrastructure.Movie.Services;
using Overoom.Infrastructure.PhotoManager;

namespace Overoom.Extensions;

public static class InfrastructureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration,
        string rootPath)
    {
        var login = configuration.GetValue<string>("SMTP:Login") ?? throw new ConfigurationException("SMTP:Login");
        var password = configuration.GetValue<string>("SMTP:Password") ??
                       throw new ConfigurationException("SMTP:Password");
        var host = configuration.GetValue<string>("SMTP:Host") ?? throw new ConfigurationException("SMTP:Host");
        var port = configuration.GetValue<int?>("SMTP:Port") ?? throw new ConfigurationException("SMTP:Port");

        var avatarPath = configuration.GetValue<string>("Thumbnails:Avatar") ??
                         throw new ConfigurationException("Thumbnails:Avatar");
        var contentPath = configuration.GetValue<string>("Thumbnails:Poster") ??
                         throw new ConfigurationException("Thumbnails:Poster");

        var kpToken = configuration.GetValue<string>("MovieApi:Kinopoisk") ??
                      throw new ConfigurationException("MovieApi:Kinopoisk");
        var videoCdnToken = configuration.GetValue<string>("MovieApi:VideoCdn") ??
                            throw new ConfigurationException("MovieApi:VideoCdn");
        var bazonToken = configuration.GetValue<string>("MovieApi:Bazon") ??
                         throw new ConfigurationException("MovieApi:Bazon");

        services.AddScoped<IEmailService, EmailService>(_ => new EmailService(login, password, host, port));

        services.AddScoped<IUserThumbnailService, UserThumbnailService>(_ =>
            new UserThumbnailService(rootPath, avatarPath));
        services.AddScoped<IPosterService, PosterService>(_ => new PosterService(rootPath, contentPath));


        services.AddSingleton<IKpResponseParser, KpResponseParser>();
        services.AddSingleton<IVideoCdnResponseParser, VideoCdnResponseParser>();
        services.AddSingleton<IBazonResponseParser, BazonResponseParser>();
        services.AddScoped<IKpApiService, KpApi>(s => new KpApi(kpToken, s.GetRequiredService<IKpResponseParser>()));
        services.AddScoped<IVideoCdnApiService, VideoCdnApi>(s =>
            new VideoCdnApi(videoCdnToken, s.GetRequiredService<IVideoCdnResponseParser>()));
        services.AddScoped<IBazonApiService, BazonApi>(s =>
            new BazonApi(bazonToken, s.GetRequiredService<IBazonResponseParser>()));
    }
}