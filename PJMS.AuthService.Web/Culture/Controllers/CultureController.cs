using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PJMS.AuthService.Abstractions.Accessories;
using PJMS.AuthService.Abstractions.Commands.Profile;

namespace PJMS.AuthService.Web.Culture.Controllers;

/// <summary>
/// Контроллер для изменения настроек культуры
/// </summary>
public class CultureController(IMediator mediator) : Controller
{
    /// <summary>
    /// Метод устанавливает куки с запрошенной культурой
    /// </summary>
    /// <param name="culture">Название культуры</param>
    /// <param name="returnUrl">Адрес на который нужно вернуться</param>
    [HttpPost]
    public async Task<IActionResult> SetCulture(string culture, string returnUrl = "/")
    {
        //todo: validate culture
        // Устанавливаем куку с информацией о языке
        Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

        // Если пользователь аутентифицирован, выполняем следующий блок кода.
        if (User.IsAuthenticated())
        {
            // Отправляем команду на изменение локализации с указанием идентификатора пользователя и локализации.
            await mediator.Send(new ChangeLocaleCommand
            {
                // Идентификатор пользователя
                UserId = User.Id(),
                
                // Локализация
                Localization = culture.GetLocalization()
            });
        }

        // Перенаправляем на локальный URL
        return LocalRedirect(returnUrl);
    }
}