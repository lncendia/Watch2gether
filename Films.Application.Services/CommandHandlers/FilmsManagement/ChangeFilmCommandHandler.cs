using Films.Application.Abstractions.Commands.FilmsManagement;
using Films.Application.Abstractions.Common.Interfaces;
using Films.Application.Services.Common;
using Films.Domain.Abstractions.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Films.Application.Services.CommandHandlers.FilmsManagement;

public class ChangeFilmCommandHandler(IUnitOfWork unitOfWork, IPosterService posterService, IMemoryCache memoryCache)
    : IRequestHandler<ChangeFilmCommand>
{
    public async Task Handle(ChangeFilmCommand request, CancellationToken cancellationToken)
    {
        var film = await memoryCache.TryGetFilmFromCache(request.Id, unitOfWork);
        if (!string.IsNullOrEmpty(request.Description)) film.Description = request.Description;
        if (!string.IsNullOrEmpty(request.ShortDescription)) film.ShortDescription = request.ShortDescription;
        if (request.RatingKp.HasValue) film.RatingKp = request.RatingKp.Value;
        if (request.RatingImdb.HasValue) film.RatingImdb = request.RatingImdb.Value;

        if (request is { CountSeasons: not null, CountEpisodes: not null })
            film.UpdateSeriesInfo(request.CountSeasons.Value, request.CountEpisodes.Value);

        Uri? poster = null;
        if (request.PosterUrl != null) poster = await posterService.SaveAsync(request.PosterUrl);
        else if (request.PosterBase64 != null) poster = await posterService.SaveAsync(request.PosterBase64);

        if (poster != null)
        {
            await posterService.DeleteAsync(film.PosterUrl);
            film.PosterUrl = poster;
        }

        if (request.CdnList != null)
        {
            foreach (var cdn in request.CdnList) film.AddOrChangeCdn(cdn);
        }

        await unitOfWork.FilmRepository.Value.UpdateAsync(film);
        await unitOfWork.SaveChangesAsync();

        memoryCache.Remove(film.Id);
    }
}