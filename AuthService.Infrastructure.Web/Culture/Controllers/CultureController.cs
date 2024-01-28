using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Infrastructure.Web.Culture.Controllers;

/// <summary>
/// Контроллер для изменения настроек культуры
/// </summary>
public class CultureController : Controller
{
    /// <summary>
    /// Метод устанавливает куки с запрошенной культурой
    /// </summary>
    /// <param name="culture">Название культуры</param>
    /// <param name="returnUrl">Адрес на который нужно вернуться</param>
    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl = "/")
    {
        // Устанавливаем куку с информацией о языке
        Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

        // Перенаправляем на локальный URL
        return LocalRedirect(returnUrl);
    }
}