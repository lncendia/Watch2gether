using MediatR;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Films.Specifications;
using Films.Domain.Playlists.Events;

namespace Films.Application.Services.EventHandlers;

public class FilmsCollectionUpdateEventHandler(IUnitOfWork unitOfWork)
    : INotificationHandler<FilmsCollectionUpdateEvent>
{
    public async Task Handle(FilmsCollectionUpdateEvent notification, CancellationToken cancellationToken)
    {
        var filmsSpec = new FilmByIdsSpecification(notification.Playlist.Films);
        var films = await unitOfWork.FilmRepository.Value.FindAsync(filmsSpec);
        var genres = films.SelectMany(x => x.Genres).GroupBy(g => g)
            .OrderByDescending(genre => genre.Count()).Select(x => x.Key).Take(5);
        notification.Playlist.UpdateGenres(genres);
        await unitOfWork.PlaylistRepository.Value.UpdateAsync(notification.Playlist);
    }
}