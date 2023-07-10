using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Users.Interfaces;
using Overoom.WEB.Contracts.Settings;
using Overoom.WEB.RoomAuthentication;

namespace Overoom.WEB.Controllers;

[Authorize(Policy = "User")]
public class SettingsController : Controller
{
    private readonly IUserProfileService _userService;

    public SettingsController(IUserProfileService userService)
    {
        _userService = userService;
    }


    [HttpGet]
    public IActionResult ChangeEmail(string message)
    {
        if (!string.IsNullOrEmpty(message)) ViewData["Alert"] = message;
        return View(new ChangeEmailParameters { Email = User.Identity!.Name! });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> ChangeEmail(ChangeEmailParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        await _userService.RequestResetEmailAsync(User.GetId(), model.Email, Url.Action(
            "AcceptChangeEmail", "Settings", null, HttpContext.Request.Scheme)!);

        return RedirectToAction("ChangeEmail",
            new
            {
                message = "Для завершения проверьте электронную почту и перейдите по ссылке, указанной в письме."
            });
    }

    [HttpGet]
    public async Task<IActionResult> AcceptChangeEmail(AcceptChangeEmailParameters model)
    {
        if (!ModelState.IsValid)  return RedirectToAction("ChangeEmail", new { message = "Некорректная ссылка." });
        await _userService.ResetEmailAsync(User.GetId(), model.Email, model.Code);
        await UpdateEmailAsync(model.Email);
        return RedirectToAction("ChangeEmail", new { message = "Почта успешно изменена." });
    }

    [HttpGet]
    public IActionResult ChangePassword(string? message)
    {
        if (!string.IsNullOrEmpty(message)) ViewData["Alert"] = message;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        await _userService.ChangePasswordAsync(User.GetId(), model.OldPassword, model.Password);
        return RedirectToAction("ChangePassword", new { message = "Пароль успешно изменен." });
    }

    [HttpGet]
    public IActionResult ChangeName(string message)
    {
        if (!string.IsNullOrEmpty(message)) ViewData["Alert"] = message;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeName(ChangeNameParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        await _userService.ChangeNameAsync(User.GetId(), model.Name);
        await UpdateNameAsync(model.Name);
        return RedirectToAction("ChangeName", new { message = "Имя успешно изменено." });
    }

    [HttpGet]
    public ActionResult ChangeAvatar(string? message)
    {
        if (!string.IsNullOrEmpty(message)) ViewData["Alert"] = message;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ChangeAvatar(IFormFile uploadedFile)
    {
        await using var stream = uploadedFile.OpenReadStream();
        var uri = await _userService.ChangeAvatarAsync(User.GetId(), stream);
        await UpdateThumbnailAsync(uri);
        return RedirectToAction("ChangeAvatar", new { message = "Аватар успешно изменён." });
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