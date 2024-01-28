using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;

namespace AuthService.Infrastructure.Web;

/// <summary>
/// Расширения
/// </summary>
public static class Extensions
{
    public static long Id(this ClaimsPrincipal principal) =>
        long.Parse(principal.FindFirstValue(JwtClaimTypes.Subject)!);
    
    public static Uri GetAvatar(this ClaimsPrincipal user) => new(user.FindFirstValue(JwtClaimTypes.Picture)!, UriKind.Relative);

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