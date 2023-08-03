using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Profile.Interfaces;
using Overoom.WEB.Contracts.Settings;
using Overoom.WEB.RoomAuthentication;
using IProfileMapper = Overoom.WEB.Mappers.Abstractions.IProfileMapper;

namespace Overoom.WEB.Controllers;

[Authorize(Policy = "User")]
public class ProfileController : Controller
{
    private readonly IUserProfileService _userService;
    private readonly IUserSettingsService _userSettings;
    private readonly IProfileMapper _mapper;

    public ProfileController(IUserProfileService userService, IProfileMapper mapper, IUserSettingsService userSettings)
    {
        _userService = userService;
        _mapper = mapper;
        _userSettings = userSettings;
        //todo: exception
    }


    public async Task<IActionResult> Index(string? message)
    {
        ViewData["Alert"] = message;
        var profile = await _userService.GetProfileAsync(User.GetId());
        var genres = await _userService.GetGenresAsync(User.GetId());
        return View(_mapper.Map(profile, genres));
    }

    public async Task<IActionResult> Ratings(int page)
    {
        try
        {
            var ratings = await _userService.GetRatingsAsync(User.GetId(), page);
            if (!ratings.Any()) return NoContent();
            var ratingModels = ratings.Select(_mapper.Map).ToList();
            return Json(ratingModels);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> ChangeEmail(ChangeEmailParameters model)
    {
        if (!ModelState.IsValid) RedirectToAction("Index", "Home", new { message = "Указаны некорректные данные." });
        await _userSettings.RequestResetEmailAsync(User.GetId(), model.Email!, Url.Action(
            "AcceptChangeEmail", "Profile", null, HttpContext.Request.Scheme)!);

        return RedirectToAction("Index", "Home",
            new
            {
                message = "Для завершения проверьте электронную почту и перейдите по ссылке, указанной в письме."
            });
    }

    [HttpGet]
    public async Task<IActionResult> AcceptChangeEmail(AcceptChangeEmailParameters model)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index", "Home", new { message = "Некорректная ссылка." });
        await _userSettings.ResetEmailAsync(User.GetId(), model.Email!, model.Code!);
        await UpdateEmailAsync(model.Email!);
        return RedirectToAction("Index", "Home", new { message = "Почта успешно изменена." });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordParameters model)
    {
        if (!ModelState.IsValid) RedirectToAction("Index", "Home", new { message = "Указаны некорректные данные." });
        await _userSettings.ChangePasswordAsync(User.GetId(), model.OldPassword!, model.Password!);
        return RedirectToAction("Index", "Home", new { message = "Пароль успешно изменен." }); //todo:exception
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeName(ChangeNameParameters model)
    {
        if (!ModelState.IsValid) RedirectToAction("Index", "Home", new { message = "Указаны некорректные данные." });
        await _userSettings.ChangeNameAsync(User.GetId(), model.Name!);
        await UpdateNameAsync(model.Name!);
        return RedirectToAction("Index", "Home", new { message = "Имя успешно изменено." });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ChangeAvatar(IFormFile uploadedFile)
    {
        await using var stream = uploadedFile.OpenReadStream();
        if (uploadedFile.Length > 15728640)
            return RedirectToAction("Index", "Home", new { message = "Размер аватара не может превышать 15 Мб." });
        var uri = await _userSettings.ChangeAvatarAsync(User.GetId(), stream);
        await UpdateThumbnailAsync(uri);
        return RedirectToAction("Index", "Home", new { message = "Аватар успешно изменён." });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ChangeAllows(ChangeAllowsParameters model)
    {
        if (!ModelState.IsValid) RedirectToAction("Index", "Home", new { message = "Указаны некорректные данные." });
        await _userSettings.ChangeAllowsAsync(User.GetId(),model.Beep, model.Scream, model.Change);
        return RedirectToAction("Index", "Home", new { message = "Разрешения успешно изменены." });
    }


    private async Task UpdateNameAsync(string name)
    {
        var claims = User.Claims.ToList();
        var nameClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
        if (nameClaim != null) claims.Remove(nameClaim);
        claims.Add(new Claim(ClaimTypes.Name, name));
        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme)));
    }

    private async Task UpdateEmailAsync(string email)
    {
        var claims = User.Claims.ToList();
        var nameClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
        if (nameClaim != null) claims.Remove(nameClaim);
        claims.Add(new Claim(ClaimTypes.Email, email));
        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme)));
    }

    private async Task UpdateThumbnailAsync(Uri uri)
    {
        var claims = User.Claims.ToList();
        var nameClaim = claims.FirstOrDefault(x => x.Type == ApplicationConstants.AvatarClaimType);
        if (nameClaim != null) claims.Remove(nameClaim);
        claims.Add(new Claim(ApplicationConstants.AvatarClaimType, uri.ToString()));
        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme)));
    }
}