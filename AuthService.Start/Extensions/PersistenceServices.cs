using AuthService.Application.Abstractions.Abstractions.AppThumbnailStore;
using AuthService.Infrastructure.Storage;
using AuthService.Start.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Start.Extensions;

public static class PersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration, string rootPath)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new ConfigurationException("ConnectionStrings:DefaultConnection");

        var avatarPath = configuration.GetRequiredValue<string>("Thumbnails");
        
        services.AddDbContext<ApplicationContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddSingleton<IThumbnailStore, UserThumbnailStore>(_ => new UserThumbnailStore(rootPath, avatarPath));
    }
}