using System.Diagnostics;
using AuthService.Application.Abstractions.Exceptions;
using AuthService.Infrastructure.Web.Exceptions;
using AuthService.Infrastructure.Web.Home.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AuthService.Infrastructure.Web.Home.Controllers;

/// <summary>
/// Контроллер домашней страницы.
/// </summary>
/// <param name="stringLocalizer">Локализатор строк.</param>
[AllowAnonymous]
public class HomeController(IStringLocalizer<HomeController> stringLocalizer) : Controller
{
    // Показывает страницу ошибки.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        // Получаем контекст ошибки из HttpContext.
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

        // Если контекст ошибки не существует.
        if (context == null) return Ok();

        // Получаем исключение из контекста ошибки.
        var requestException = context.Error;

        // Определяем ресурс для отображения сообщения об ошибке на основе типа исключения.
        string? resource;
        switch (requestException)
        {
            case EmailSendException:
            case UserNotFoundException:
            case ExternalLoginException:
            case LoginAlreadyAssociatedException:
            case QueryParameterMissingException:
            case LoginNotFoundException:
            case InvalidCodeException:
            case EmailFormatException:
            case UserNameLengthException:
            case UserNameFormatException:
            case EmailAlreadyTakenException:
            case UserLockoutException:
                resource = requestException.GetType().Name;
                break;
            default:
                resource = "DefaultException";
                break;
        }

        // Возвращаем представление с моделью ErrorViewModel, передавая сообщение об ошибке и идентификатор запроса.
        return View(new ErrorViewModel
        {
            // Сообщение с ошибкой
            Message = stringLocalizer[resource],

            // Идентификатор запроса
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}