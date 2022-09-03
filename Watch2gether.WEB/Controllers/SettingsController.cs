using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Watch2gether.Application.Abstractions.Exceptions.Users;
using Watch2gether.Application.Abstractions.Interfaces.Users;
using Watch2gether.Domain.Users.Exceptions;
using Watch2gether.WEB.Models.Settings;

namespace Watch2gether.WEB.Controllers;

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
            switch (ex)
            {
                case UserNotFoundException:
                    ModelState.AddModelError("", "Пользователь с таким email не найден");
                    break;
                case EmailException:
                    ModelState.AddModelError("", "Произошла ошибка при отправке письма");
                    break;
                case ArgumentException:
                    ModelState.AddModelError("", "Некорректные данные");
                    break;
            }

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
            switch (ex)
            {
                case UserNotFoundException:
                    ModelState.AddModelError("", "Пользователь не найден");
                    break;
                case ArgumentException:
                    ModelState.AddModelError("", "Некорректные данные");
                    break;
                default:
                    ModelState.AddModelError("", "Произошла ошибка при изменении пароля");
                    break;
            }

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
            await _userService.ChangeNameAsync(User.Identity!.Name!, model.Name);
            return RedirectToAction("ChangeName", new {message = "Имя успешно изменено."});
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case UserNotFoundException:
                    ModelState.AddModelError("", "Пользователь не найден");
                    break;
                case InvalidNicknameException:
                    ModelState.AddModelError("", "Некорректные данные");
                    break;
                default:
                    ModelState.AddModelError("", "Произошла ошибка при изменении имени");
                    break;
            }

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
            await _userService.ChangeAvatarAsync(User.Identity!.Name!, stream);
            return RedirectToAction("ChangeAvatar", new {message = "Аватар успешно изменён."});
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case UserNotFoundException:
                    ModelState.AddModelError("", "Пользователь не найден");
                    break;
                case ThumbnailSaveException:
                    ModelState.AddModelError("", "Некорректный формат изображения");
                    break;
                default:
                    ModelState.AddModelError("", "Произошла ошибка при изменении аватара");
                    break;
            }
            return View();
        }
    }
}