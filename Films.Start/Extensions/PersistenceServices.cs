using Films.Start.Exceptions;
using Microsoft.EntityFrameworkCore;
using Films.Start.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Start.Infrastructure.ApplicationData;
using Films.Start.Infrastructure.Storage;
using Films.Start.Infrastructure.Storage.Context;

namespace Films.Start.Extensions;

public static class PersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new ConfigurationException("ConnectionStrings:DefaultConnection");
        services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(connectionString));
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
