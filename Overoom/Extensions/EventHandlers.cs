using MediatR;
using Overoom.Application.Services.Movie.EventHandlers;
using Overoom.Domain.Ratings.Events;

namespace Overoom.Extensions;

public static class EventHandlers
{
    public static void AddEventHandlers(this IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddTransient<INotificationHandler<NewRatingEvent>, NewRatingEventHandler>();
    }
}