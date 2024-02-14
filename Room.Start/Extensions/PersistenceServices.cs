using Microsoft.EntityFrameworkCore;
using Room.Domain.Abstractions.Interfaces;
using Room.Infrastructure.Storage;
using Room.Infrastructure.Storage.Context;

namespace Room.Start.Extensions;

public static class PersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetRequiredValue<string>("ConnectionStrings:DefaultConnection");
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString,
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}