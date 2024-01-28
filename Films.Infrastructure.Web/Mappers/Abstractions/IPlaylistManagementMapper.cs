using Films.Application.Abstractions.PlaylistsManagement.DTOs;
using Films.Infrastructure.Web.Contracts.PlaylistManagement.Change;
using Films.Infrastructure.Web.Contracts.PlaylistManagement.Load;
using Films.Infrastructure.Web.Models.PlaylistManagement;

namespace Films.Infrastructure.Web.Mappers.Abstractions;

public interface IPlaylistManagementMapper
{
    ChangeParameters Map(PlaylistDto dto);
    ChangeDto Map(ChangeParameters parameters);
    LoadDto Map(LoadParameters parameters);
    PlaylistViewModel Map(PlaylistShortDto dto);
}