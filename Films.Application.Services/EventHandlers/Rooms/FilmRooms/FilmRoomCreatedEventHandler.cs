using Films.Application.Abstractions.Common.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.FilmRooms.Events;
using Films.Domain.Rooms.FilmRooms.Specifications;
using Films.Domain.Rooms.YoutubeRooms.Specifications;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.BaseRooms;
using Overoom.IntegrationEvents.Rooms.FilmRooms;

namespace Films.Application.Services.EventHandlers.Rooms.FilmRooms;

public class FilmRoomCreatedEventHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    : INotificationHandler<FilmRoomCreatedDomainEvent>
{
    public async Task Handle(FilmRoomCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var filmRoomsSpecification = new FilmRoomByUserSpecification(notification.Owner.Id);
        var filmRoomsCount = await unitOfWork.FilmRoomRepository.Value.CountAsync(filmRoomsSpecification);

        var youtubeRoomsSpecification = new YoutubeRoomByUserSpecification(notification.Owner.Id);
        var youtubeRoomsCount = await unitOfWork.YoutubeRoomRepository.Value.CountAsync(youtubeRoomsSpecification);

        if (filmRoomsCount + youtubeRoomsCount >= 3) throw new MaxNumberRoomsReachedException();
        
        await publishEndpoint.Publish(new FilmRoomCreatedIntegrationEvent
        {
            Id = notification.Room.Id,
            Title = notification.Film.Title,
            CdnUrl = notification.Film.CdnList.First(c => c.Name == notification.Room.CdnName).Url,
            IsSerial = notification.Film.IsSerial,
            Owner = new Viewer
            {
                Id = notification.Owner.Id,
                PhotoUrl = notification.Owner.PhotoUrl,
                Name = notification.Owner.UserName,
                Beep = notification.Owner.Allows.Beep,
                Scream = notification.Owner.Allows.Scream,
                Change = notification.Owner.Allows.Change
            }
        }, context => context.SetRoutingKey(notification.Room.ServerId.ToString()), cancellationToken);
    }
}