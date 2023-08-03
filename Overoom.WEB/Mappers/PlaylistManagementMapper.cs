// using Overoom.Application.Abstractions.PlaylistsManagement.DTOs;
// using Overoom.WEB.Contracts.PlaylistManagement;
// using Overoom.WEB.Mappers.Abstractions;
// using Overoom.WEB.Models.PlaylistManagement;
//
// namespace Overoom.WEB.Mappers;
//
// public class PlaylistManagementMapper : IPlaylistManagementMapper
// {
//     public ChangeParameters Map(PlaylistDto dto)
//     {
//         return new ChangeParameters
//         {
//            I
//         };
//     }
//
//     public ChangeDto Map(ChangeParameters parameters)
//     {
//         var uri = parameters.NewPosterUri == null ? null : new Uri(parameters.NewPosterUri);
//         var cdns = parameters.Cdns
//             .Select(x => new CdnDto(x.Type!.Value, new Uri(x.Uri!), x.Quality!, x.Voices.Select(v => v.Name!).ToList()))
//             .ToList();
//         return new ChangeDto(parameters.Id, parameters.Description, parameters.ShortDescription,
//             uri, parameters.Rating, cdns, parameters.CountSeasons, parameters.CountEpisodes,
//             parameters.NewPoster?.OpenReadStream());
//     }
//
//     public PlaylistViewModel Map(PlaylistShortDto dto) => new(dto.Id, dto.PosterUri, dto.Name, dto.Year);
// }