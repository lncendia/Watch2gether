using Films.Application.Abstractions.Common.Interfaces;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Infrastructure.Storage;
using Films.Infrastructure.Storage.Context;
using Microsoft.EntityFrameworkCore;

namespace Films.Start.Extensions;

public static class PersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration,
        string rootPath)
    {
        var connectionString = configuration.GetRequiredValue<string>("ConnectionStrings:DefaultConnection");

        var contentPath = configuration.GetRequiredValue<string>("Thumbnails");

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
        services.AddDbContext<ApplicationDbContext2>(options => options.UseSqlite("DataSource=app.db;Cache=Shared"));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPosterService, PosterService>(_ => new PosterService(rootPath, contentPath));
    }
}