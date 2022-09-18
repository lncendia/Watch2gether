using Microsoft.EntityFrameworkCore;
using Watch2gether.Domain.Abstractions.Repositories;
using Watch2gether.Domain.Abstractions.Repositories.UnitOfWorks;
using Watch2gether.Infrastructure.ApplicationData;
using Watch2gether.Infrastructure.PersistentStorage;
using Watch2gether.Infrastructure.PersistentStorage.Context;

namespace Watch2gether.Extensions;

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