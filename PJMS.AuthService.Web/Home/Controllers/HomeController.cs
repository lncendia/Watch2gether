using System.Diagnostics;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Web.Attributes;
using PJMS.AuthService.Web.Exceptions;
using PJMS.AuthService.Web.Home.ViewModels;

namespace PJMS.AuthService.Web.Home.Controllers;

/// <summary>
/// Контроллер домашней страницы.
/// </summary>
[AllowAnonymous]
[SecurityHeaders]
public class HomeController : Controller
{
    /// <summary>
    /// Сервис взаимодействия с IdentityServer.
    /// </summary>
    private readonly IIdentityServerInteractionService _interaction;

    /// <summary>
    /// Веб-хостовая среда.
    /// </summary>
    private readonly IWebHostEnvironment _environment;

    /// <summary>
    /// Логгер.
    /// </summary>
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Локализатор строк.
    /// </summary>
    private readonly IStringLocalizer<HomeController> _stringLocalizer;

    /// <summary>
    /// Конструктор класса HomeController.
    /// </summary>
    /// <param name="interaction">Сервис взаимодействия с IdentityServer.</param>
    /// <param name="environment">Веб-хостовая среда.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="stringLocalizer">Локализатор строк.</param>
    public HomeController(IIdentityServerInteractionService interaction,
        IWebHostEnvironment environment, ILogger<HomeController> logger,
        IStringLocalizer<HomeController> stringLocalizer)
    {
        _interaction = interaction;
        _environment = environment;
        _logger = logger;
        _stringLocalizer = stringLocalizer;
    }

    /// <summary>
    /// Действие для отображения домашней страницы.
    /// </summary>
    /// <returns>Результат действия.</returns>
    public IActionResult Index()
    {
        // Если приложение находится в режиме разработки, показываем страницу.
        if (_environment.IsDevelopment())
        {
            // Показывать только в режиме разработки.
            return View();
        }

        // Записываем информационное сообщение в лог о том, что домашняя страница отключена в рабочей версии.
        _logger.LogInformation("Домашняя страница отключена в рабочей версии. Возврат 404");

        // Возвращаем ошибку 404 - страница не найдена.
        return NotFound();
    }

    // Показывает страницу ошибки.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error(string? errorId)
    {
        // Получаем контекст ошибки из HttpContext.
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

        // Если контекст ошибки существует.
        if (context != null)
        {
            // Получаем исключение из контекста ошибки.
            var requestException = context.Error;

            // Логгируем исключение
            _logger.LogError(requestException, "Ошибка при обработке запроса");

            // Определяем ресурс для отображения сообщения об ошибке на основе типа исключения.
            string? resource;
            switch (requestException)
            {
                case IdentityContextException:
                case EmailSendException:
                case UserNotFoundException:
                case LoginAlreadyAssociatedException:
                case QueryParameterMissingException:
                case LoginNotFoundException:
                case InvalidCodeException:
                case EmailFormatException:
                case UserNameLengthException:
                case EmailAlreadyTakenException:
                case UserLockoutException:
                case TwoFactorAlreadyEnabledException:
                case ThumbnailSaveException:
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
                Message = _stringLocalizer[resource],

                // Идентификатор запроса
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                
                // Url возврата
                ReturnUrl = "/"
            });
        }

        // Получаем сведения об ошибке с IdentityServer на основе идентификатора ошибки.
        var message = await _interaction.GetErrorContextAsync(errorId);

        // Если сведения об ошибке существуют.
        if (message != null)
        {
            // Возвращаем представление с моделью ErrorViewModel, передавая сообщение об ошибке и идентификатор запроса.
            return View(new ErrorViewModel
            {
                // Сообщение с ошибкой
                Message = message.ErrorDescription,

                // Идентификатор запроса
                RequestId = message.RequestId,
                
                // Url возврата
                ReturnUrl = message.RedirectUri ?? "/"
            });
        }

        // Возвращаем ответ Ok.
        return Ok();
    }
}