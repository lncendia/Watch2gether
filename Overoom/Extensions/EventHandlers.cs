using MediatR;
using Overoom.Application.Services.Movie.EventHandlers;
using Overoom.Application.Services.PlaylistsManagement.EventHandlers;
using Overoom.Domain.Films.Events;
using Overoom.Domain.Playlists.Events;
using Overoom.Domain.Ratings.Events;

namespace Overoom.Extensions;

public static class EventHandlers
{
    public static void AddEventHandlers(this IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddTransient<INotificationHandler<NewRatingEvent>, NewRatingEventHandler>();
        services.AddTransient<INotificationHandler<NewFilmEvent>, NewFilmEventHandler>();
        services.AddTransient<INotificationHandler<FilmsCollectionUpdateEvent>, FilmsCollectionUpdateEventHandler>();
        services
            .AddTransient<INotificationHandler<NewRatingEvent>,
                Application.Services.Profile.EventHandlers.NewRatingEventHandler>();
    }
}