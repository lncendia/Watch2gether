using Overoom.Application.Abstractions.PlaylistsManagement.DTOs;
using Overoom.WEB.Contracts.PlaylistManagement.Change;
using Overoom.WEB.Contracts.PlaylistManagement.Load;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models.PlaylistManagement;
using FilmParameters = Overoom.WEB.Contracts.PlaylistManagement.Change.FilmParameters;

namespace Overoom.WEB.Mappers;

public class PlaylistManagementMapper : IPlaylistManagementMapper
{
    public ChangeParameters Map(PlaylistDto dto)
    {
        return new ChangeParameters
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Films = dto.Films.Select(f => new FilmParameters
            {
                Id = f.Id,
                Description = f.Description,
                Name = f.Name,
                Uri = f.PosterUri.ToString()
            }).ToList()
        };
    }

    public ChangeDto Map(ChangeParameters parameters)
    {
        var uri = parameters.NewPosterUri == null ? null : new Uri(parameters.NewPosterUri);
        return new ChangeDto(parameters.Id, parameters.Name, parameters.Description, uri,
            parameters.NewPoster?.OpenReadStream(),
            parameters.Films.Where(x => x.Id.HasValue).Select(x => x.Id!.Value).ToList().AsReadOnly());
    }

    public LoadDto Map(LoadParameters parameters)
    {
        var uri = parameters.PosterUri == null ? null : new Uri(parameters.PosterUri);
        return new LoadDto(parameters.Name!, parameters.Description!, uri,
            parameters.Poster?.OpenReadStream(), parameters.Films.Select(x => x.Id!.Value).ToList());
    }

    public PlaylistViewModel Map(PlaylistShortDto dto) => new(dto.Id, dto.PosterUri, dto.Name);
}