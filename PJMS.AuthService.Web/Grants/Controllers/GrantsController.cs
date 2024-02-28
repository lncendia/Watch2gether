using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PJMS.AuthService.Web.Attributes;
using PJMS.AuthService.Web.Grants.ViewModels;
using PJMS.AuthService.Web.Services.Abstractions;

namespace PJMS.AuthService.Web.Grants.Controllers;

/// <summary>
/// Этот образец контроллера позволяет пользователю отзывать гранты, предоставленные клиентам.
/// </summary>
[Authorize]
[SecurityHeaders]
public class GrantsController : Controller
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IClientStore _clients;
    private readonly IResourceStore _resources;
    private readonly IEventService _events;

    /// <summary>
    /// Локализатор
    /// </summary>
    private readonly IScopeLocalizer _localizer;

    public GrantsController(IIdentityServerInteractionService interaction, IClientStore clients,
        IResourceStore resources, IEventService events, IScopeLocalizer localizer)
    {
        _interaction = interaction;
        _clients = clients;
        _resources = resources;
        _events = events;
        _localizer = localizer;
    }

    /// <summary>
    /// Получить список разрешений
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await BuildViewModelAsync());
    }

    /// <summary>
    /// Обработка обратной передачи для отзыва клиента
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Revoke(string clientId)
    {
        // Отзыв согласия пользователя для указанного идентификатора клиента
        await _interaction.RevokeUserConsentAsync(clientId);

        // Вызов события о отзыве предоставленных разрешений
        await _events.RaiseAsync(new GrantsRevokedEvent(User.GetSubjectId(), clientId));

        // Перенаправление на действие "Index"
        return RedirectToAction("Index");
    }

    /// <summary>
    /// Построить ViewModel Async
    /// </summary>
    /// <returns></returns>
    private async Task<GrantsViewModel> BuildViewModelAsync()
    {
        // Получение всех разрешений пользователя
        var grants = await _interaction.GetAllUserGrantsAsync();

        // Создание списка моделей представления разрешений
        var grantsViewModels = new List<GrantViewModel>();

        // Итерация по каждому разрешению
        foreach (var grant in grants)
        {
            // Поиск клиента по идентификатору клиента в разрешении
            var client = await _clients.FindClientByIdAsync(grant.ClientId);

            // Если клиент не найден - пропускаем
            if (client == null) continue;

            // Поиск ресурсов по областям разрешения
            var resources = await _resources.FindResourcesByScopeAsync(grant.Scopes);

            // Формируем список областей Identity
            var identityScopes = resources.IdentityResources.Select(CreateIdentityScopeViewModel);

            // Формируем список областей Api
            var apiScopes = resources.ApiScopes.Select(CreateApiScopeViewModel);

            // Добавляем к Api неограниченный доступ, если он есть
            if (resources.OfflineAccess) apiScopes = apiScopes.Append(CreateOfflineAccessScopeViewModel());

            // Создание нового объекта GrantViewModel 
            var item = new GrantViewModel
            {
                // Установка идентификатора клиента в свойство ClientId 
                ClientId = client.ClientId,

                // Установка имени клиента в свойство ClientName, если оно не равно null, иначе установка ClientId 
                ClientName = client.ClientName ?? client.ClientId,

                // Установка URL клиента в свойство ClientUrl 
                ClientUrl = client.ClientUri,

                // Установка URL логотипа клиента в свойство ClientLogoUrl 
                ClientLogoUrl = client.LogoUri,

                // Установка описания в свойство Description 
                Description = grant.Description,

                // Установка даты создания в свойство Created 
                Created = grant.CreationTime,

                // Установка даты истечения в свойство Expires 
                Expires = grant.Expiration,

                // Установка имен идентификационных разрешений в свойство IdentityGrantNames 
                IdentityGrantNames = identityScopes,

                // Установка имен разрешений API в свойство ApiGrantNames 
                ApiGrantNames = apiScopes
            };

            // Добавление модели представления разрешения в список
            grantsViewModels.Add(item);
        }

        // Возвращение модели представления со списком разрешений
        return new GrantsViewModel
        {
            // Установка списка разрешений
            Grants = grantsViewModels
        };
    }

    /// <summary>
    /// Создать область видимости ViewModel
    /// </summary>
    /// <param name="resource">Ресурс IdentityServer</param>
    /// <returns>Модель скоупа</returns>
    private string CreateIdentityScopeViewModel(IdentityResource resource)
    {
        // Локализуем ресурс
        var localized = _localizer.Localize(resource);

        // Возвращаем имя
        return localized.Name;
    }

    /// <summary>
    /// Создать область видимости ViewModel
    /// </summary>
    /// <param name="apiScope"></param>
    /// <returns>Модель скоупа</returns>
    public string CreateApiScopeViewModel(ApiScope apiScope)
    {
        // Локализуем api область
        var localized = _localizer.Localize(apiScope);

        // Возвращаем имя
        return localized.Name;
    }

    /// <summary>
    /// Получить область автономного доступа
    /// </summary>
    /// <returns>Модель скоупа</returns>
    private string CreateOfflineAccessScopeViewModel()
    {
        // Локализуем область offline
        var localized = _localizer.LocalizeOffline();

        // Возвращаем имя
        return localized.Name;
    }
}