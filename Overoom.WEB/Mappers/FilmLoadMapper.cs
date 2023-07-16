using Overoom.Application.Abstractions.FilmsManagement.DTOs;
using Overoom.WEB.Contracts.FilmLoad;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models.FilmLoad;

namespace Overoom.WEB.Mappers;

public class FilmLoadMapper : IFilmLoadMapper
{
    public LoadDto Map(LoadParameters parameters)
    {
        var uri = parameters.PosterUri == null ? null : new Uri(parameters.PosterUri);
        var cdnList = parameters.Cdns
            .Select(x => new CdnDto(x.Type!.Value, new Uri(x.Uri!), x.Quality!, x.Voices.Select(s => s.Name!).ToList())).ToList();
        var actors = parameters.Actors.Select(x => new ValueTuple<string, string?>(x.Name!, x.Description)).ToList();
        var countries = parameters.Countries.Select(x => x.Name).ToList();
        var genres = parameters.Genres.Select(x => x.Name).ToList();
        var directors = parameters.Directors.Select(x => x.Name).ToList();
        var screenwriters = parameters.Screenwriters.Select(x => x.Name).ToList();
        return new LoadDto(parameters.Name!, parameters.Description!, parameters.ShortDescription,
            parameters.Rating!.Value, parameters.Year!.Value, parameters.Type!.Value, uri, parameters.Poster?.OpenReadStream(), genres!,
            actors, countries!, directors!, screenwriters!, cdnList, parameters.CountSeasons, parameters.CountEpisodes);
    }

    public FilmViewModel Map(FilmDto dto)
    {
        return new FilmViewModel(dto.Name, dto.Description, dto.ShortDescription, dto.Rating, dto.Year, dto.Type,
            dto.PosterUri, dto.Genres, dto.Actors.Select(x => new ActorViewModel(x.name, x.description)).ToList(),
            dto.Countries, dto.Directors, dto.Screenwriters, dto.CountSeasons,
            dto.CountEpisodes);
    }
}