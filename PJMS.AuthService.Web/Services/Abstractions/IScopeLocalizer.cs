using IdentityServer4.Models;

namespace PJMS.AuthService.Web.Services.Abstractions;

/// <summary>
/// Определяет методы для локализации разрешений.
/// </summary>
public interface IScopeLocalizer
{
    /// <summary>
    /// Локализует разрешение доступа к API.
    /// </summary>
    /// <param name="scope">Разрешение доступа к API.</param>
    /// <param name="parsedParameter">Обработанный параметр.</param>
    /// <returns>Локализованное разрешение.</returns>
    LocalizedScope Localize(ApiScope scope, string? parsedParameter = null);
    
    /// <summary>
    /// Локализует идентификационное разрешение.
    /// </summary>
    /// <param name="resource">Идентификационное разрешение.</param>
    /// <returns>Локализованное разрешение.</returns>
    LocalizedScope Localize(IdentityResource resource);
    
    /// <summary>
    /// Локализует оффлайн-разрешение.
    /// </summary>
    /// <returns>Локализованное разрешение оффлайн-доступа.</returns>
    LocalizedScope LocalizeOffline();
}