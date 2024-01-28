using Films.Application.Services.EventHandlers;
using MediatR;
using Films.Start.Application.Services.Films.EventHandlers;
using Films.Start.Application.Services.PlaylistsManagement.EventHandlers;
using Films.Start.Domain.Films.Events;
using Films.Start.Domain.Playlists.Events;
using Films.Start.Domain.Ratings.Events;

namespace Films.Start.Extensions;

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
                NewRatingUserEventHandler>();
    }
}