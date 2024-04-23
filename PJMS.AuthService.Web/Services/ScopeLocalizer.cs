using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Localization;
using PJMS.AuthService.Web.Services.Abstractions;

namespace PJMS.AuthService.Web.Services;

/// <summary>
/// Класс для локализации разрешений.
/// </summary>
/// <param name="localizer">Локалайзер.</param>
public class ScopeLocalizer(IStringLocalizer<ScopeLocalizer> localizer) : IScopeLocalizer
{
    /// <summary>
    /// Метод для локализации разрешения доступа к API.
    /// </summary>
    /// <param name="scope">Разрешение доступа к API.</param>
    /// <param name="parsedParameter">Параметр разрешения, если есть.</param>
    /// <returns>Локализованное отображаемое имя и описание идентификационного разрешения.</returns>
    public LocalizedScope Localize(ApiScope scope, string? parsedParameter)
    {
        // Получение локализованного отображаемого имени разрешения доступа к API из ресурсов 
        var displayName = localizer["AccessToService"] + ' ' + localizer[scope.Name];

        // Если значение параметра разрешения не пустое, добавление его к отображаемому имени 
        if (!string.IsNullOrWhiteSpace(parsedParameter))
        {
            displayName += ":" + parsedParameter;
        }

        // Получение локализованного описания разрешения доступа к API из ресурсов 
        var description = localizer[scope.Name + "Description"];

        // Если описание не найдено в ресурсах, установка значения null 
        if (description.ResourceNotFound) description = null;

        // Возвращаем локализованное отображаемое имя и описание разрешения доступа к API
        return new LocalizedScope
        {
            Name = displayName,
            Description = description!
        };
    }

    /// <summary>
    /// Метод для локализации.
    /// </summary>
    /// <param name="resource">Ресурсы для локализации.</param>
    /// <returns>Локализованное отображаемое имя и описание идентификационного разрешения.</returns>
    public LocalizedScope Localize(IdentityResource resource)
    {
        // Получение локализованного имени идентификационного разрешения из ресурсов 
        var displayName = localizer[resource.Name];

        // Получение локализованного описания идентификационного разрешения из ресурсов 
        var description = localizer[resource.Name + "Description"];

        // Если описание не найдено в ресурсах, установка значения null 
        if (description.ResourceNotFound) description = null;

        // Возвращаем локализованное отображаемое имя и описание идентификационного разрешения
        return new LocalizedScope
        {
            Name = displayName,
            Description = description!
        };
    }

    /// <summary>
    /// Метод локализации оффлайн-разрешения.
    /// </summary>
    /// <returns>Локализованное отображаемое имя и описание оффлайн-разрешения.</returns>
    public LocalizedScope LocalizeOffline()
    {
        // Установка значения константы "OfflineAccess" в переменную name 
        const string name = IdentityServerConstants.StandardScopes.OfflineAccess;
        
        // Возвращаем локализованное отображаемое имя и описание оффлайн-разрешения
        return new LocalizedScope
        {
            Name = localizer[name],
            Description = localizer[name + "Description"]
        };
    }
}