using Films.Start.Application.Abstractions.Authentication.Interfaces;
using Films.Start.Application.Abstractions.Comments.Interfaces;
using Films.Start.Application.Abstractions.Films.Interfaces;
using Films.Start.Application.Abstractions.FilmsInformation.Interfaces;
using Films.Start.Application.Abstractions.FilmsLoading;
using Films.Start.Application.Abstractions.FilmsManagement.Interfaces;
using Films.Start.Application.Abstractions.Movie.Interfaces;
using Films.Start.Application.Abstractions.Playlists.Interfaces;
using Films.Start.Application.Abstractions.PlaylistsManagement.Interfaces;
using Films.Start.Application.Abstractions.Profile.Interfaces;
using Films.Start.Application.Abstractions.Rooms.Interfaces;
using Films.Start.Application.Abstractions.StartPage.Interfaces;
using Films.Start.Application.Services.Authentication;
using Films.Start.Application.Services.Comments;
using Films.Start.Application.Services.Films;
using Films.Start.Application.Services.FilmsCatalog;
using Films.Start.Application.Services.FilmsInformation;
using Films.Start.Application.Services.FilmsLoading;
using Films.Start.Application.Services.FilmsManagement;
using Films.Start.Application.Services.PlaylistsCatalog;
using Films.Start.Application.Services.PlaylistsManagement;
using Films.Start.Application.Services.Profile;
using Films.Start.Application.Services.Rooms;
using Films.Start.Application.Services.StartPage;

namespace Films.Start.Extensions;

public static class ApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICommentManager, CommentManager>();
        services.AddScoped<IFilmManager, FilmManager>();
        services.AddScoped<IFilmManagementService, FilmManagementService>();
        services.AddScoped<IPlaylistManagementService, PlaylistManagementService>();
        services.AddScoped<IFilmsManager, FilmsManager>();
        services.AddScoped<IPlaylistsManager, PlaylistsManager>();
        services.AddScoped<IFilmRoomManager, FilmRoomManager>();
        services.AddScoped<IYoutubeRoomManager, YoutubeRoomManager>();
        services.AddScoped<IStartPageService, StartPageService>();
        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<ISettingsService, SettingsService>();
        services.AddScoped<IFilmInfoService, FilmInfoService>();
        services.AddScoped<IFilmAutoLoader, FilmAutoLoader>();
        services.AddMemoryCache();
    }
}