using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.Playlists.Interfaces;
using Overoom.WEB.Contracts.Playlists;
using IPlaylistsMapper = Overoom.WEB.Mappers.Abstractions.IPlaylistsMapper;

namespace Overoom.WEB.Controllers;

public class PlaylistsController : Controller
{
    private readonly IPlaylistsManager _playlistsManager;
    private readonly IPlaylistsMapper _playlistsMapper;

    public PlaylistsController(IPlaylistsMapper mapper, IPlaylistsManager playlistsManager)
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