using Overoom.Application.Abstractions.Authentication.Interfaces;
using Overoom.Application.Abstractions.Comments.Interfaces;
using Overoom.Application.Abstractions.Films.Interfaces;
using Overoom.Application.Abstractions.FilmsInformation.Interfaces;
using Overoom.Application.Abstractions.FilmsLoading;
using Overoom.Application.Abstractions.FilmsManagement.Interfaces;
using Overoom.Application.Abstractions.Movie.Interfaces;
using Overoom.Application.Abstractions.Playlists.Interfaces;
using Overoom.Application.Abstractions.PlaylistsManagement.Interfaces;
using Overoom.Application.Abstractions.Profile.Interfaces;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.Application.Abstractions.StartPage.Interfaces;
using Overoom.Application.Services.Authentication;
using Overoom.Application.Services.Comments;
using Overoom.Application.Services.Films;
using Overoom.Application.Services.FilmsCatalog;
using Overoom.Application.Services.FilmsInformation;
using Overoom.Application.Services.FilmsLoading;
using Overoom.Application.Services.FilmsManagement;
using Overoom.Application.Services.PlaylistsCatalog;
using Overoom.Application.Services.PlaylistsManagement;
using Overoom.Application.Services.Profile;
using Overoom.Application.Services.Rooms;
using Overoom.Application.Services.StartPage;

namespace Overoom.Extensions;

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