using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.User.Exceptions;
using Overoom.Application.Abstractions.User.Interfaces;
using Overoom.Domain.User.Exceptions;
using Overoom.WEB.Models.Settings;

namespace Overoom.WEB.Controllers;

[Authorize(Policy = "Identity.Application")]
public class SettingsController : Controller
{
    private readonly IUserSecurityService _userSettingsService;
    private readonly IUserParametersService _userService;

    public SettingsController(IUserParametersService userService, IUserSecurityService userSettingsService)
    {
        _userService = userService;
        _userSettingsService = userSettingsService;
    }

    [HttpGet]
    public IActionResult ChangeEmail(string message)
    {
        if (!string.IsNullOrEmpty(message)) ViewData["Alert"] = message;
        return View(new ChangeEmailViewModel {Email = User.Identity!.Name!});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        try
        {
            await _userSettingsService.RequestResetEmailAsync(User.Identity!.Name!, model.Email, Url.Action(
                "AcceptChangeEmail",
                "Settings", null,
                HttpContext.Request.Scheme)!);

            return RedirectToAction("ChangeEmail",
                new
                {
                    message = "Для завершения проверьте электронную почту и перейдите по ссылке, указанной в письме."
                });
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                UserNotFoundException => "Пользователь не найден",
                EmailException => "Произошла ошибка при отправке письма",
                ArgumentException => "Некорректные данные",
                _ => "Произошла ошибка при изменении почты"
            };

            ModelState.AddModelError("", text);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> AcceptChangeEmail(string? email, string? code)
    {
        if (code == null || email == null)
            return RedirectToAction("ChangeEmail", new {message = "Ссылка недействительна."});

        try
        {
            await _userSettingsService.ResetEmailAsync(User.Identity!.Name!, email, code);
            return RedirectToAction("ChangeEmail", new {message = "Почта успешно изменена."});
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                UserNotFoundException => "Пользователь с таким email не найден",
                InvalidCodeException => "Ссылка недействительна",
                _ => "Произошла ошибка при смене почты"
            };

            return RedirectToAction("ChangeEmail", new {message = text});
        }
    }

    [HttpGet]
    public IActionResult ChangePassword(string message)
    {
        if (!string.IsNullOrEmpty(message)) ViewData["Alert"] = message;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        try
        {
            await _userSettingsService.ChangePasswordAsync(User.Identity!.Name!, model.OldPassword,
                model.Password);
            return RedirectToAction("ChangePassword", new {message = "Пароль успешно изменен."});
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                UserNotFoundException => "Пользователь не найден",
                ArgumentException => "Некорректные данные",
                _ => "Произошла ошибка при изменении пароля"
            };

            ModelState.AddModelError("", text);
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult ChangeName(string message)
    {
        if (!string.IsNullOrEmpty(message)) ViewData["Alert"] = message;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeName(ChangeNameViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        try
        {
            await _userService.ChangeNameAsync(TODO, model.Name);
            return RedirectToAction("ChangeName", new {message = "Имя успешно изменено."});
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                UserNotFoundException => "Пользователь не найден",
                InvalidNicknameException => "Некорректные данные",
                _ => "Произошла ошибка при изменении имени"
            };

            ModelState.AddModelError("", text);
            return View(model);
        }
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
        try
        {
            await using var stream = uploadedFile.OpenReadStream();
            await _userService.ChangeAvatarAsync(TODO, stream);
            return RedirectToAction("ChangeAvatar", new {message = "Аватар успешно изменён."});
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                UserNotFoundException => "Пользователь не найден",
                ThumbnailSaveException => "Некорректный формат изображения",
                _ => "Произошла ошибка при изменении аватара"
            };

            ModelState.AddModelError("", text);
            return View();
        }
    }
}