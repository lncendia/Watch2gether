using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;

namespace PJMS.AuthService.Web;

/// <summary>
/// Расширения для работы с аутентификацией
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Возвращает идентификатор пользователя из объекта ClaimsPrincipal.
    /// </summary>
    /// <param name="principal">Объект ClaimsPrincipal.</param>
    /// <returns>Идентификатор пользователя.</returns>
    public static Guid Id(this ClaimsPrincipal principal) => Guid.Parse(principal.FindFirstValue(JwtClaimTypes.Subject)!);

    /// <summary>
    /// Проверяет, предназначена ли схема для внешней аутентификации
    /// </summary>
    /// <returns></returns>
    public static bool IsOauthScheme(this AuthenticationScheme scheme)
    {
        // Получаем тип OAuthHandler из пространства имен Microsoft.AspNetCore.Authentication.OAuth 
        var typeOauth2 = typeof(Microsoft.AspNetCore.Authentication.OAuth.OAuthHandler<>);

        // Получаем тип RemoteAuthenticationHandler из текущего пространства имен 
        var typeOauth = typeof(RemoteAuthenticationHandler<>);

        try
        {
            // Получаем базовый обобщенный тип обработчика схемы аутентификации 
            var genericType = scheme.HandlerType.BaseType?.GetGenericTypeDefinition();

            // Проверяем, является ли базовый обобщенный тип OAuthHandler или RemoteAuthenticationHandler 
            return genericType == typeOauth || genericType == typeOauth2;
        }
        catch
        {
            // Если возникло исключение, возвращаем false 
            return false;
        }
    }
}