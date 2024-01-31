using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Films.Application.Abstractions.PlaylistsManagement.Interfaces;
using Films.Infrastructure.Web.Contracts.PlaylistManagement;
using Films.Infrastructure.Web.Contracts.PlaylistManagement.Change;
using Films.Infrastructure.Web.Contracts.PlaylistManagement.Load;
using Abstractions_IPlaylistManagementMapper = Films.Infrastructure.Web.Mappers.Abstractions.IPlaylistManagementMapper;
using IPlaylistManagementMapper = Films.Infrastructure.Web.Mappers.Abstractions.IPlaylistManagementMapper;
using Mappers_Abstractions_IPlaylistManagementMapper = Films.Infrastructure.Web.Mappers.Abstractions.IPlaylistManagementMapper;

namespace Films.Infrastructure.Web.Controllers;

[Authorize(Policy = "Admin")]
public class PlaylistManagementController : Controller
{
    private readonly IPlaylistManagementService _managementService;
    private readonly Mappers_Abstractions_IPlaylistManagementMapper _mapper;

    public PlaylistManagementController(IPlaylistManagementService managementService, Mappers_Abstractions_IPlaylistManagementMapper mapper)
    {
        _managementService = managementService;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult Index()
    {
        return View(new PlaylistsSearchParameters());
    }

    [HttpGet]
    public async Task<IActionResult> PlaylistsList(PlaylistsSearchParameters model)
    {
        if (!ModelState.IsValid) return NoContent();
        try
        {
            var playlists = await _managementService.FindAsync(model.Page, model.Query);
            if (!playlists.Any()) return NoContent();
            var playlistsModels = playlists.Select(_mapper.Map).ToList();
            return Json(playlistsModels);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpGet]
    public IActionResult Load()
    {
        return View(new LoadParameters());
    }

    [HttpPost]
    public async Task<IActionResult> Load(LoadParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        if (model.Poster?.Length > 15728640)
        {
            ModelState.AddModelError("", "Размер постера не может превышать 15 Мб");
            return View(model);
        }

        var film = _mapper.Map(model);

        await _managementService.LoadAsync(film);
        return RedirectToAction("Index", "Home", new { message = "Подборка успешно создана" });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var playlist = await _managementService.GetAsync(id);
        return View(_mapper.Map(playlist));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ChangeParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        if (model.NewPoster?.Length > 15728640)
        {
            ModelState.AddModelError("", "Размер постера не может превышать 15 Мб");
            return View(model);
        }

        await _managementService.ChangeAsync(_mapper.Map(model));
        return RedirectToAction("Index", "Home", new { message = "Подборка изменена" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid playlistId)
    {
        await _managementService.DeleteAsync(playlistId);
        return RedirectToAction("Index", "PlaylistManagement", new { message = "Подборка удалена" });
    }
}