using Films.Application.Abstractions.Common.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using MediatR;
using Films.Domain.Rooms.FilmRooms.Events;
using Films.Domain.Rooms.FilmRooms.Specifications;
using Films.Domain.Rooms.YoutubeRooms.Specifications;
using MassTransit;

namespace Films.Application.Services.EventHandlers;

public class FilmRoomCreatedEventHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint) : INotificationHandler<FilmRoomCreatedDomainEvent>
{
    public async Task Handle(FilmRoomCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var filmRoomsSpecification = new FilmRoomByUserSpecification(notification.Owner.Id);
        var filmRoomsCount = await unitOfWork.FilmRoomRepository.Value.CountAsync(filmRoomsSpecification);

        var youtubeRoomsSpecification = new YoutubeRoomByUserSpecification(notification.Owner.Id);
        var youtubeRoomsCount = await unitOfWork.YoutubeRoomRepository.Value.CountAsync(youtubeRoomsSpecification);

        if (filmRoomsCount + youtubeRoomsCount >= 3) throw new MaxNumberRoomsReachedException();

        publishEndpoint.Publish();
    }
}