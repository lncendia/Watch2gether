using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.Authentication.DTOs;
using Overoom.Application.Abstractions.Authentication.Entities;
using Overoom.Application.Abstractions.Authentication.Exceptions;
using Overoom.Application.Abstractions.Authentication.Interfaces;
using Overoom.Application.Abstractions.Common.Exceptions;
using Overoom.Domain.Users.Exceptions;
using Overoom.WEB.Contracts.Accounts;
using Overoom.WEB.Models.Account;

namespace Overoom.WEB.Controllers;

public class AccountController : Controller
{
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly SignInManager<UserData> _signInManager;

    public AccountController(IUserAuthenticationService userAuthenticationService, SignInManager<UserData> signInManager)
    {
        _userAuthenticationService = userAuthenticationService;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Register(string returnUrl = "/")
    {
        return View(new RegisterParameters {ReturnUrl = returnUrl});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        try
        {
            await _userAuthenticationService.CreateAsync(new UserCreateDto(model.Name!, model.Password!, model.Email!),
                Url.Action("AcceptCode", "Account", null, HttpContext.Request.Scheme)!);

            return RedirectToAction("Continue", new { returnUrl = model.ReturnUrl });
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case UserAlreadyExistException:
                    ModelState.AddModelError("", "Пользователь с таким email уже существует");
                    break;
                case NicknameLengthException:
                    ModelState.AddModelError("",
                        "Длина имени должна составлять от 3 до 20 символов");
                    break;
                case NicknameFormatException:
                    ModelState.AddModelError("",
                        "Имя может содержать только латинские или кириллические буквы, цифры, пробелы и символы подчеркивания");
                    break;
                case PasswordLengthException:
                    ModelState.AddModelError("",
                        "Длина пароля должна составлять от 8 до 30 символов");
                    break;
                case PasswordFormatException:
                    ModelState.AddModelError("",
                        "Пароль должен содержать буквы, цифры и специальные символы и не может иметь разрывов");
                    break;
                case UserCreationException exception:
                    ModelState.AddModelError("", exception.Message);
                    break;
                default:
                    throw;
            }

            return View(model);
        }
    }

    public IActionResult Continue(string returnUrl = "/")
    {
        return View(new ContinueViewModel(returnUrl));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> AcceptCode(string? email, string? code)
    {
        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Index", "Home", new { message = "Ссылка недействительна." });
        }

        var user = await _userAuthenticationService.CodeAuthenticateAsync(email, code);
        await _signInManager.SignInAsync(user, true);
        return RedirectToAction("Index", "Home", new { message = "Почта успешно подтверждена." });
    }

    public IActionResult LoginOauth(string provider, string returnUrl = "/")
    {
        //todo: check scheme
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/")
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null) return RedirectToAction(nameof(Login));
        var user = await _userAuthenticationService.ExternalAuthenticateAsync(info);
        await _signInManager.SignInAsync(user, true);
        await HttpContext.SignOutAsync(info.AuthenticationProperties);
        return Redirect(returnUrl);
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = "/")
    {
        return View(new LoginParameters { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        try
        {
            var user = await _userAuthenticationService.PasswordAuthenticateAsync(model.Email!, model.Password!);
            await _signInManager.SignInAsync(user, model.RememberMe);
            return Redirect(model.ReturnUrl);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case UserNotFoundException:
                    ModelState.AddModelError("", "Пользователь с таким email не найден");
                    break;
                case InvalidPasswordException:
                    ModelState.AddModelError("", "Неверный пароль");
                    break;
                default:
                    throw;
            }

            return View(model);
        }
    }

    [Authorize(Policy = "User")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home", new { message = "Вы вышли из аккаунта." });
    }

    [HttpGet]
    public IActionResult ResetPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        await _userAuthenticationService.RequestResetPasswordAsync(model.Email!, Url.Action(
            "NewPassword", "Account", null, HttpContext.Request.Scheme)!);
        return RedirectToAction("Login", new
        {
            message =
                "Для восстановления пароля проверьте электронную почту и перейдите по ссылке, указанной в письме."
        });
    }

    [HttpGet]
    public IActionResult NewPassword(string? email, string? code)
    {
        if (email is null && code is null)
        {
            return RedirectToAction("Login", new { message = "Ссылка недействительна." });
        }

        return View(new NewPasswordParameters { Email = email!, Code = code! });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NewPassword(NewPasswordParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        await _userAuthenticationService.ResetPasswordAsync(model.Email!, model.Code!, model.Password!);
        return RedirectToAction("Login", new { message = "Пароль успешно изменен." });
    }
}