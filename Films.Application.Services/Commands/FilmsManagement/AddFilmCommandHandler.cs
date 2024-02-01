using Films.Application.Abstractions.Commands.FilmsManagement;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Common.Interfaces;
using MediatR;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Films.Entities;

namespace Films.Application.Services.Commands.FilmsManagement;

public class AddFilmCommandHandler(IUnitOfWork unitOfWork, IPosterService posterService)
    : IRequestHandler<AddFilmCommand, Guid>
{
    public async Task<Guid> Handle(AddFilmCommand request, CancellationToken cancellationToken)
    {
        Uri? poster;
        if (request.PosterUrl != null) poster = await posterService.SaveAsync(request.PosterUrl);
        else if (request.PosterBase64 != null) poster = await posterService.SaveAsync(request.PosterBase64);
        else throw new PosterMissingException();
        var film = new Film(request.Type, request.CountSeasons, request.CountEpisodes)
        {
            Title = request.Title,
            Description = request.Description,
            ShortDescription = request.ShortDescription,
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
        await unitOfWork.FilmRepository.Value.AddAsync(film);
        await unitOfWork.SaveChangesAsync();
        return film.Id;
    }
}