using Watch2gether.Domain.Abstractions.Interfaces;
using Watch2gether.Domain.Services.Services;

namespace Watch2gether.Extensions;

public static class DomainServices
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IFilmRoomService, FilmRoomService>();
    }
}