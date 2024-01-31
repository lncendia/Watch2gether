using Microsoft.AspNetCore.Mvc;
using Films.Application.Abstractions.Playlists.Interfaces;
using Films.Infrastructure.Web.Contracts.Playlists;
using Abstractions_IPlaylistsMapper = Films.Infrastructure.Web.Mappers.Abstractions.IPlaylistsMapper;
using IPlaylistsMapper = Films.Infrastructure.Web.Mappers.Abstractions.IPlaylistsMapper;
using Mappers_Abstractions_IPlaylistsMapper = Films.Infrastructure.Web.Mappers.Abstractions.IPlaylistsMapper;

namespace Films.Infrastructure.Web.Controllers;

public class PlaylistsController : Controller
{
    private readonly IPlaylistsManager _playlistsManager;
    private readonly Mappers_Abstractions_IPlaylistsMapper _playlistsMapper;

    public PlaylistsController(Mappers_Abstractions_IPlaylistsMapper mapper, IPlaylistsManager playlistsManager)
    {
        _playlistsMapper = mapper;
        _playlistsManager = playlistsManager;
    }
    

    public IActionResult Playlists()
    {
        return View();
    }
    
    
    // public IActionResult PlaylistSearch(SearchParameters model)
    // {
    //     return View(_playlistsMapper.Map(model));
    // }

    [HttpGet]
    public async Task<IActionResult> PlaylistsList(PlaylistsSearchParameters searchParameters)
    {
        if (!ModelState.IsValid) return NoContent();
        try
        {
            var playlists = await _playlistsManager.FindAsync(_playlistsMapper.Map(searchParameters));
            if (!playlists.Any()) return NoContent();
            var playlistsModels = playlists.Select(_playlistsMapper.Map).ToList();
            return Json(playlistsModels);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}