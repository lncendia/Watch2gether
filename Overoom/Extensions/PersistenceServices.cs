using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Abstractions.Repositories;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Infrastructure.ApplicationData;
using Overoom.Infrastructure.PersistentStorage;
using Overoom.Infrastructure.PersistentStorage.Context;

namespace Overoom.Extensions;

public static class PersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlite(connectionString));
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}