using Films.Application.Abstractions.Commands.FilmsManagement;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Posters;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Films;
using MediatR;

namespace Films.Application.Services.CommandHandlers.FilmsManagement;

public class AddFilmCommandHandler(IUnitOfWork unitOfWork, IPosterStore posterStore)
    : IRequestHandler<AddFilmCommand, Guid>
{
    public async Task<Guid> Handle(AddFilmCommand request, CancellationToken cancellationToken)
    {
        Uri? poster;
        if (request.PosterUrl != null) poster = await posterStore.SaveAsync(request.PosterUrl);
        else if (request.PosterBase64 != null) poster = await posterStore.SaveAsync(request.PosterBase64);
        else throw new PosterMissingException();
        var film = new Film(request.IsSerial, request.CountSeasons, request.CountEpisodes)
        {
            Title = request.Title,
            Description = request.Description,
            Year = request.Year,
            PosterUrl = poster,
            Genres = request.Genres,
            Countries = request.Countries,
            Directors = request.Directors,
            Actors = request.Actors,
            Screenwriters = request.Screenwriters,
            CdnList = request.CdnList,
            RatingImdb = request.RatingImdb,
            RatingKp = request.RatingKp
        };

        if (!string.IsNullOrEmpty(request.ShortDescription)) film.ShortDescription = request.ShortDescription;
        await unitOfWork.FilmRepository.Value.AddAsync(film);
        await unitOfWork.SaveChangesAsync();
        return film.Id;
    }
}