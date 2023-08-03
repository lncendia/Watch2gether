using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.PlaylistsManagement.Interfaces;
using Overoom.WEB.Contracts.PlaylistManagement;
using Overoom.WEB.Contracts.PlaylistManagement.Change;
using Overoom.WEB.Contracts.PlaylistManagement.Load;
using IPlaylistManagementMapper = Overoom.WEB.Mappers.Abstractions.IPlaylistManagementMapper;

namespace Overoom.WEB.Controllers;

[Authorize(Policy = "Admin")]
public class PlaylistManagementController : Controller
{
    private readonly IPlaylistManagementService _managementService;
    private readonly IPlaylistManagementMapper _mapper;

    public PlaylistManagementController(IPlaylistManagementService managementService, IPlaylistManagementMapper mapper)
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
        return RedirectToAction("Index", "playlistManagement", new { message = "Подборка удалена" });
    }
}