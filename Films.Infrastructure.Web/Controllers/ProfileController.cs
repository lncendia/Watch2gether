using Films.Infrastructure.Web.Contracts.Settings;
using Films.Infrastructure.Web.Mappers.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Films.Infrastructure.Web.Authentication;
using Abstractions_IProfileMapper = Films.Infrastructure.Web.Mappers.Abstractions.IProfileMapper;

namespace Films.Infrastructure.Web.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IProfileService _service;
    private readonly ISettingsService _settings;
    private readonly IProfileMapper _mapper;

    public ProfileController(IProfileService service, IProfileMapper mapper, ISettingsService settings)
    {
        _service = service;
        _mapper = mapper;
        _settings = settings;
        //todo: exception
    }


    public async Task<IActionResult> Index(string? message)
    {
        ViewData["Alert"] = message;
        var profile = await _service.GetProfileAsync(User.GetId());
        var genres = await _service.GetGenresAsync(User.GetId());
        return View(_mapper.Map(profile, genres));
    }

    public async Task<IActionResult> Ratings(int page)
    {
        try
        {
            var ratings = await _service.GetRatingsAsync(User.GetId(), page);
            if (!ratings.Any()) return NoContent();
            var ratingModels = ratings.Select(_mapper.Map).ToList();
            return Json(ratingModels);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ChangeAllows(ChangeAllowsParameters model)
    {
        if (!ModelState.IsValid) RedirectToAction("Index", "Home", new { message = "Указаны некорректные данные." });
        await _settings.ChangeAllowsAsync(User.GetId(),model.Beep, model.Scream, model.Change);
        return RedirectToAction("Index", "Home", new { message = "Разрешения успешно изменены." });
    }
}