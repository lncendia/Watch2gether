using Overoom.Application.Abstractions.PlaylistsManagement.DTOs;
using Overoom.WEB.Contracts.PlaylistManagement.Change;
using Overoom.WEB.Contracts.PlaylistManagement.Load;
using Overoom.WEB.Models.PlaylistManagement;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IPlaylistManagementMapper
{
    ChangeParameters Map(PlaylistDto dto);
    ChangeDto Map(ChangeParameters parameters);
    LoadDto Map(LoadParameters parameters);
    PlaylistViewModel Map(PlaylistShortDto dto);
}