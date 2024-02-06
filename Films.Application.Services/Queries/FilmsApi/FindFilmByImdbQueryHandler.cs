using Films.Application.Abstractions.Queries.FilmsApi;
using Films.Application.Abstractions.Queries.FilmsApi.DTOs;
using Films.Application.Abstractions.Services.MovieApi.Interfaces;
using MediatR;

namespace Films.Application.Services.Queries.FilmsApi;

public class FindFilmByImdbQueryHandler(IKpApiService kpApi, IVideoCdnApiService videoCdnApi)
    : IRequestHandler<FindFilmByImdbQuery, FilmApiDto>
{
    public async Task<FilmApiDto> Handle(FindFilmByImdbQuery request, CancellationToken cancellationToken)
    {
        var films = await kpApi.FindByImdbAsync(request.Imdb, token: cancellationToken);
        if (films.Count == 0) throw new NotImplementedException();
        var kpId = films.First().KpId;
        var film = await kpApi.GetAsync(kpId, cancellationToken);
        var staff = await kpApi.GetActorsAsync(kpId, cancellationToken);
        var seasons = film.Serial ? await kpApi.GetSeasonsAsync(kpId, cancellationToken) : null;
        
        var videoCdn = await videoCdnApi.GetInfoAsync(kpId, cancellationToken);

        return Mapper.Map(film, staff, videoCdn, seasons);
    }
}