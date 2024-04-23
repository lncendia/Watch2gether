using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PJMS.AuthService.Web.Attributes;

/// <summary>
/// Добавляет заголовки безопасности (csp, referer и т.д.)
/// </summary>
public class SecurityHeadersAttribute : ActionFilterAttribute
{
    ///<summary> 
    /// Метод OnResultExecuting вызывается перед выполнением результата действия контроллера. 
    ///</summary> 
    ///<param name="context">Контекст выполнения результата.</param> 
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        // Получаем результат выполнения запроса
        var result = context.Result;

        // Если результат не является ViewResult, прервать выполнение метода.
        if (result is not ViewResult) return;

        // Установка заголовка X-Content-Type-Options для предотвращения "MIME type sniffing".
        if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
        {
            context.HttpContext.Response.Headers.XContentTypeOptions = "nosniff";
        }

        // Установка заголовка X-Frame-Options для защиты от кликджекинга.
        if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
        {
            context.HttpContext.Response.Headers.XFrameOptions = "SAMEORIGIN";
        }

        // Определение политики безопасности содержимого (Content Security Policy - CSP).
        var csp =
            "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";

        // Добавление директивы upgrade-insecure-requests, чтобы все запросы были через HTTPS.
        csp += "upgrade-insecure-requests;";

        // Разрешение отображения изображений только из источников 'self' и данных в формате data:.
        csp += "img-src 'self' data:;";

        // Разрешение подключения стилей только из 'self' и 'unsafe-inline'.
        csp += "style-src 'self' 'unsafe-inline'";

        // Установка заголовка Content-Security-Policy для современных браузеров.
        if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
        {
            context.HttpContext.Response.Headers.ContentSecurityPolicy = csp;
        }

        // Установка заголовка X-Content-Security-Policy для поддержки старых версий IE.
        if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
        {
            context.HttpContext.Response.Headers["X-Content-Security-Policy"] = csp;
        }

        // Установка заголовка Referrer-Policy для ограничения отправки информации referrer.
        const string referrerPolicy = "no-referrer";
        if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
        {
            context.HttpContext.Response.Headers["Referrer-Policy"] = referrerPolicy;
        }
    }
}