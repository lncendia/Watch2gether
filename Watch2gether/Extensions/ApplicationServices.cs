using Watch2gether.Application.Abstractions.Interfaces.Comments;
using Watch2gether.Application.Abstractions.Interfaces.Films;
using Watch2gether.Application.Abstractions.Interfaces.Playlists;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.Application.Abstractions.Interfaces.Users;
using Watch2gether.Application.Services.Services;
using Watch2gether.Application.Services.Services.Comments;
using Watch2gether.Application.Services.Services.Films;
using Watch2gether.Application.Services.Services.Playlists;
using Watch2gether.Application.Services.Services.Rooms;
using Watch2gether.Application.Services.Services.Users;

namespace Watch2gether.Extensions;

public static class ApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserParametersService, UserParametersService>();
        services.AddScoped<IUserSecurityService, UserSecurityService>();
        services.AddScoped<IRoomManager, RoomManager>();
        services.AddScoped<ICommentManager, CommentManager>();
        services.AddScoped<IRoomDeleterManager, RoomDeleterManager>();
        services.AddScoped<IFilmManager, FilmManager>();
        services.AddScoped<IPlaylistManager, PlaylistManager>();
        services.AddScoped<IFilmLoaderService, FilmLoaderService>();
    }
}