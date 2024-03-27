using Films.Application.Abstractions.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.FilmRooms.Events;
using Films.Domain.Rooms.FilmRooms.Specifications;
using Films.Domain.Rooms.YoutubeRooms.Specifications;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.BaseRooms;
using Overoom.IntegrationEvents.Rooms.FilmRooms;

namespace Films.Application.Services.EventHandlers.FilmRooms;

public class FilmRoomCreatedEventHandler(
    IUnitOfWork unitOfWork,
    IRequestClient<FilmRoomCreatedIntegrationEvent> client)
    : INotificationHandler<FilmRoomCreatedDomainEvent>
{
    public async Task Handle(FilmRoomCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var filmRoomsSpecification = new FilmRoomByUserSpecification(notification.Owner.Id);
        var filmRoomsCount = await unitOfWork.FilmRoomRepository.Value.CountAsync(filmRoomsSpecification);

        var youtubeRoomsSpecification = new YoutubeRoomByUserSpecification(notification.Owner.Id);
        var youtubeRoomsCount = await unitOfWork.YoutubeRoomRepository.Value.CountAsync(youtubeRoomsSpecification);

        if (filmRoomsCount + youtubeRoomsCount >= 3) throw new MaxNumberRoomsReachedException();

        var cdn = notification.Film.CdnList.First(c => c.Name == notification.Room.CdnName);

        var integrationEvent = new FilmRoomCreatedIntegrationEvent
        {
            Id = notification.Room.Id,
            Title = notification.Film.Title,
            CdnName = cdn.Name,
            CdnUrl = cdn.Url,
            IsSerial = notification.Film.IsSerial,
            Owner = new Viewer
            {
                Id = notification.Owner.Id,
                PhotoUrl = notification.Owner.PhotoUrl,
                Name = notification.Owner.Username,
                Beep = notification.Owner.Allows.Beep,
                Scream = notification.Owner.Allows.Scream,
                Change = notification.Owner.Allows.Change
            }
        };

        await client.GetResponse<FilmRoomAcceptedIntegrationEvent>(integrationEvent, context =>
            context.UseExecute(c => c.SetRoutingKey(notification.Room.ServerId.ToString())), cancellationToken);
    }
}