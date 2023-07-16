using Overoom.Application.Abstractions.FilmsManagement.DTOs;
using Overoom.WEB.Contracts.FilmManagement;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models.FilmManagement;
using CdnParameters = Overoom.WEB.Contracts.FilmManagement.CdnParameters;

namespace Overoom.WEB.Mappers;

public class FilmManagementMapper : IFilmManagementMapper
{
    public ChangeParameters Map(GetDto dto)
    {
        var cdns = dto.CdnList.Select(x => new CdnParameters
        {
            Quality = x.Quality, Type = x.Type, Uri = x.Uri.ToString(),
            Voices = x.Voices.Select(v => new VoiceParameters { Name = v }).ToList()
        }).ToList();
        return new ChangeParameters
        {
            Id = dto.FilmId, CountEpisodes = dto.CountEpisodes, CountSeasons = dto.CountSeasons,
            Description = dto.Description, Rating = dto.Rating,
            ShortDescription = dto.ShortDescription, Cdns = cdns
        };
    }

    public ChangeDto Map(ChangeParameters parameters)
    {
        var uri = parameters.NewPosterUri == null ? null : new Uri(parameters.NewPosterUri);
        var cdns = parameters.Cdns
            .Select(x => new CdnDto(x.Type!.Value, new Uri(x.Uri!), x.Quality!, x.Voices.Select(v => v.Name!).ToList())).ToList();
        return new ChangeDto(parameters.Id, parameters.Description, parameters.ShortDescription,
            uri, parameters.Rating, cdns, parameters.CountSeasons, parameters.CountEpisodes,
            parameters.NewPoster?.OpenReadStream());
    }

    public FilmViewModel Map(FilmShortDto dto) => new(dto.Id, dto.PosterUri, dto.Name, dto.Year);
}