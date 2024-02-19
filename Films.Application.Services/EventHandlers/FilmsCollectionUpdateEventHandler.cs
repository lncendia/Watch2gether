using Films.Domain.Abstractions.Interfaces;
using MediatR;
using Films.Domain.Films.Specifications;
using Films.Domain.Playlists.Events;

namespace Films.Application.Services.EventHandlers;

public class FilmsCollectionUpdateEventHandler(IUnitOfWork unitOfWork)
    : INotificationHandler<FilmsCollectionUpdateDomainEvent>
{
    public async Task Handle(FilmsCollectionUpdateDomainEvent notification, CancellationToken cancellationToken)
    {
        var filmsSpec = new FilmsByIdsSpecification(notification.Playlist.Films);
        var films = await unitOfWork.FilmRepository.Value.FindAsync(filmsSpec);
        var genres = films.SelectMany(x => x.Genres).GroupBy(g => g)
            .OrderByDescending(genre => genre.Count()).Select(x => x.Key).Take(5);
        notification.Playlist.UpdateGenres(genres);
        await unitOfWork.PlaylistRepository.Value.UpdateAsync(notification.Playlist);
    }
}