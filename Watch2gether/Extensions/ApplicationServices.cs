using Watch2gether.Application.Abstractions.Interfaces.Films;
using Watch2gether.Application.Abstractions.Interfaces.Playlists;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.Application.Abstractions.Interfaces.Users;
using Watch2gether.Application.Services.Services;

namespace Watch2gether.Extensions;

public static class ApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserParametersService, UserParametersService>();
        services.AddScoped<IUserSecurityService, UserSecurityService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IRoomDeleterManager, RoomDeleterManager>();
        services.AddScoped<IFilmManager, FilmManager>();
        services.AddScoped<IPlaylistManager, PlaylistManager>();
        services.AddScoped<IFilmLoaderService, FilmLoaderService>();
    }
}