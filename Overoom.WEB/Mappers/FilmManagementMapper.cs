using Overoom.Application.Abstractions.FilmsManagement.DTOs;
using Overoom.WEB.Contracts.FilmManagement.Change;
using Overoom.WEB.Contracts.FilmManagement.Load;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models.FilmManagement;

namespace Overoom.WEB.Mappers;

public class FilmManagementMapper : IFilmManagementMapper
{
    public ChangeParameters Map(FilmDto dto)
    {
        var cdns = dto.CdnList.Select(x => new Contracts.FilmManagement.Change.CdnParameters
        {
            Quality = x.Quality, Type = x.Type, Uri = x.Uri.ToString(),
            Voices = x.Voices.Select(v => new Contracts.FilmManagement.Change.VoiceParameters { Name = v }).ToList()
        }).ToList();
        return new ChangeParameters
        {
            Id = dto.FilmId, CountEpisodes = dto.CountEpisodes, CountSeasons = dto.CountSeasons,
            Description = dto.Description, Rating = dto.Rating,
            ShortDescription = dto.ShortDescription, Cdns = cdns, Name = dto.Title
        };
    }

    public ChangeDto Map(ChangeParameters parameters)
    {
        var uri = parameters.NewPosterUri == null ? null : new Uri(parameters.NewPosterUri);
        var cdns = parameters.Cdns
            .Select(x => new CdnDto(x.Type!.Value, new Uri(x.Uri!), x.Quality!, x.Voices.Select(v => v.Name!).ToList()))
            .ToList();
        return new ChangeDto(parameters.Id, parameters.Description, parameters.ShortDescription,
            uri, parameters.Rating, cdns, parameters.CountSeasons, parameters.CountEpisodes,
            parameters.NewPoster?.OpenReadStream());
    }

    public FilmViewModel Map(FilmShortDto dto) => new(dto.Id, dto.PosterUri, dto.Name, dto.Year);


    public LoadDto Map(LoadParameters parameters)
    {
        var uri = parameters.PosterUri == null ? null : new Uri(parameters.PosterUri);
        var cdnList = parameters.Cdns
            .Select(x => new CdnDto(x.Type!.Value, new Uri(x.Uri!), x.Quality!, x.Voices.Select(s => s.Name!).ToList()))
            .ToList();
        var actors = parameters.Actors.Select(x => new ValueTuple<string, string?>(x.Name!, x.Description)).ToList();
        var countries = parameters.Countries.Select(x => x.Name).ToList();
        var genres = parameters.Genres.Select(x => x.Name).ToList();
        var directors = parameters.Directors.Select(x => x.Name).ToList();
        var screenwriters = parameters.Screenwriters.Select(x => x.Name).ToList();
        return new LoadDto(parameters.Name!, parameters.Description!, parameters.ShortDescription,
            parameters.Rating!.Value, parameters.Year!.Value, parameters.Type!.Value, uri,
            parameters.Poster?.OpenReadStream(), genres!,
            actors, countries!, directors!, screenwriters!, cdnList, parameters.CountSeasons, parameters.CountEpisodes);
    }

    public FilmInfoViewModel Map(Application.Abstractions.FilmsInformation.DTOs.FilmDto dto)
    {
        return new FilmInfoViewModel(dto.Name, dto.Description, dto.ShortDescription, dto.Rating, dto.Year, dto.Type,
            dto.PosterUri, dto.Genres, dto.Actors.Select(x => new ActorViewModel(x.name, x.description)).ToList(),
            dto.Countries, dto.Directors, dto.Screenwriters, dto.CountSeasons,
            dto.CountEpisodes, dto.Cdn.Select(x => new CdnViewModel(x.Type, x.Uri, x.Quality, x.Voices)).ToList());
    }
}