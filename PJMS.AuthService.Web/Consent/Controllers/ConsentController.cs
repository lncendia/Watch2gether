using System.Web;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PJMS.AuthService.Web.Attributes;
using PJMS.AuthService.Web.Consent.InputModels;
using PJMS.AuthService.Web.Consent.ViewModels;
using PJMS.AuthService.Web.Exceptions;
using PJMS.AuthService.Web.Services.Abstractions;

namespace PJMS.AuthService.Web.Consent.Controllers;

/// <summary>
/// Этот контроллер обрабатывает пользовательский интерфейс согласия
/// </summary>
[Authorize]
[SecurityHeaders]
public class ConsentController : Controller
{
    /// <summary>
    /// Сервис взаимодействия с Identity Server.
    /// </summary>
    private readonly IIdentityServerInteractionService _interaction;

    /// <summary>
    /// Сервис событий.
    /// </summary>
    private readonly IEventService _events;

    /// <summary>
    /// Локализатор
    /// </summary>
    private readonly IScopeLocalizer _localizer;

    /// <summary>
    /// Локализатор
    /// </summary>
    private readonly IStringLocalizer<ConsentController> _stringLocalizer;

    /// <summary>
    /// Конструктор класса ConsentController.
    /// </summary>
    /// <param name="interaction">Сервис взаимодействия с Identity Server.</param>
    /// <param name="events">Сервис событий.</param>
    /// <param name="localizer">Локализатор областей</param>
    /// <param name="stringLocalizer">Локализатор</param>
    public ConsentController(IIdentityServerInteractionService interaction, IEventService events,
        IScopeLocalizer localizer, IStringLocalizer<ConsentController> stringLocalizer)
    {
        _interaction = interaction;
        _events = events;
        _localizer = localizer;
        _stringLocalizer = stringLocalizer;
    }

    /// <summary>
    /// Показывает экран согласия
    /// </summary>
    /// <param name="returnUrl"></param>
    [HttpGet]
    public async Task<IActionResult> Index(string returnUrl = "/")
    {
        // Проверяем, находимся ли мы в контексте запроса авторизации
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        // если у нас нет действительного контекста - вызываем исключение
        if (context == null) throw new IdentityContextException();

        //строим вью-модель
        var vm = CreateConsentViewModel(returnUrl, context);

        //передаем модель во вью
        return View(vm);
    }

    /// <summary>
    /// Обрабатывает обратную передачу экрана согласия
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ConsentInputModel model)
    {
        // Проверяем, находимся ли мы в контексте запроса авторизации
        var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

        // Если у нас нет действительного контекста - вызываем исключение
        if (context == null) throw new IdentityContextException();
        
        // Устанавливаем в строку запроса закодированную returnUrl, чтоб при изменении локали открылась корректная ссылка (смотреть _Culture.cshtml)
        HttpContext.Request.QueryString = new QueryString("?ReturnUrl=" + HttpUtility.UrlEncode(model.ReturnUrl));

        // Если пользователь не согласовал ни одну область
        if (model.ScopesConsented.Count == 0)
        {
            // Добавляем ошибку в состояние модели
            ModelState.AddModelError("", _stringLocalizer["NoOneConsented"]);
            
            // Строим модель
            var vm = BuildViewModel(model, context);

            // Возвращаем представление
            return View(vm);
        }

        // Обрабатываем согласие
        await ProcessConsent(model, context);

        // Перенаправляем назад в клиента
        return Redirect(model.ReturnUrl);
    }

    /// <summary>
    /// Обрабатывает отмену согласия
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DenyConsent(string returnUrl = "/")
    {
        // Проверяем, находимся ли мы в контексте запроса авторизации
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        // Если у нас нет действительного контекста - вызываем исключение
        if (context == null) throw new IdentityContextException();

        // Создаем объект ConsentResponse с указанием, что пользователь отклонил согласие
        var grantedConsent = new ConsentResponse { Error = AuthorizationError.AccessDenied };

        // Отправляем результат согласия в Identity Server
        await _interaction.GrantConsentAsync(context, grantedConsent);

        // Вызываем событие
        await _events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(),
            context.Client.ClientId, context.ValidatedResources.RawScopeValues));

        // Перенаправляем по url возврата
        return Redirect(returnUrl);
    }

    /*****************************************/
    /* helper APIs for the ConsentController */
    /*****************************************/
    /// <summary>
    /// Согласие процесса
    /// </summary>
    /// <param name="model">Модель из контроллера</param>
    /// <param name="request">Контекст авторизации</param>
    private async Task ProcessConsent(ConsentInputModel model, AuthorizationRequest request)
    {
        var grantedConsent = new ConsentResponse
        {
            RememberConsent = model.RememberConsent,
            ScopesValuesConsented = model.ScopesConsented,
            Description = model.Description
        };

        // Вызываем событие
        await _events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(),
            request.Client.ClientId, request.ValidatedResources.RawScopeValues,
            grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));

        // Отправляем результат согласия в Identity Server
        await _interaction.GrantConsentAsync(request, grantedConsent);
    }

    /// <summary>
    /// Создает ViewModel согласия
    /// </summary>
    /// <param name="context">Контекст авторизации</param>
    /// <param name="model">Модель из контроллера</param>
    /// <returns></returns>
    private ConsentViewModel BuildViewModel(ConsentInputModel model, AuthorizationRequest context)
    {
        //создаем базовую модель согласия
        var consent = CreateConsentViewModel(model.ReturnUrl, context);

        //устанавливаем флаг запомнить из модели
        consent.RememberConsent = model.RememberConsent;

        //устанавливаем описание из модели
        consent.Description = model.Description;

        //убираем отметки у убранных областей
        foreach (var consentIdentityScope in consent.IdentityScopes.Where(consentIdentityScope =>
                     !model.ScopesConsented.Contains(consentIdentityScope.Value)))
        {
            consentIdentityScope.Checked = false;
        }

        //убираем отметки у убранных областей
        foreach (var consentApiScope in consent.ApiScopes.Where(consentApiScope =>
                     !model.ScopesConsented.Contains(consentApiScope.Value)))
        {
            consentApiScope.Checked = false;
        }

        //возвращаем модель
        return consent;
    }

    /// <summary>
    /// Создает ViewModel согласия
    /// </summary>
    /// <param name="returnUrl">Url возврата</param>
    /// <param name="request">Запрос авторизации IdentityServer</param>
    /// <returns>Модель представления для страницы согласия</returns>
    private ConsentViewModel CreateConsentViewModel(string returnUrl, AuthorizationRequest request)
    {
        // Создание массива моделей представления идентификационных разрешений на основе ресурсов IdentityResources
        var identityScopes = request.ValidatedResources.Resources.IdentityResources
            .Select(CreateIdentityScopeViewModel)
            .ToArray();

        // Создание списка моделей представления разрешений API
        var apiScopes = new List<ScopeViewModel>();

        // Итерация по разобранным разрешениям в запросе
        foreach (var parsedScope in request.ValidatedResources.ParsedScopes)
        {
            // Поиск соответствующего разрешения API на основе разобранного имени
            var apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);

            // Если разрешение API не найдено, пропуск текущей итерации
            if (apiScope == null) continue;

            // Создание модели представления разрешения API на основе разобранного разрешения и найденного разрешения API
            var scopeVm = CreateApiScopeViewModel(parsedScope, apiScope);

            // Добавление модели представления разрешения API в список
            apiScopes.Add(scopeVm);
        }

        // Если в ресурсах присутствует разрешение на оффлайн-доступ, добавление модели представления оффлайн-доступа в список разрешений API
        if (request.ValidatedResources.Resources.OfflineAccess)
        {
            apiScopes.Add(CreateOfflineAccessScopeViewModel());
        }

        // Создание нового объекта ConsentViewModel 
        return new ConsentViewModel
        {
            // Установка URL возврата в свойство ReturnUrl 
            ReturnUrl = returnUrl,

            // Установка имени клиента в свойство ClientName, если оно не равно null, иначе установка ClientId 
            ClientName = request.Client.ClientName ?? request.Client.ClientId,

            // Установка URL клиента в свойство ClientUrl 
            ClientUrl = request.Client.ClientUri,

            // Установка URL логотипа клиента в свойство ClientLogoUrl 
            ClientLogoUrl = request.Client.LogoUri,

            // Установка разрешения запоминания согласия клиента в свойство AllowRememberConsent 
            AllowRememberConsent = request.Client.AllowRememberConsent,

            // Установка идентификационных разрешений в свойство IdentityScopes 
            IdentityScopes = identityScopes,

            // Установка разрешений API в свойство ApiScopes 
            ApiScopes = apiScopes
        };
    }

    /// <summary>
    /// Создать область видимости ViewModel
    /// </summary>
    /// <param name="resource">Ресурс IdentityServer</param>
    /// <returns>Модель скоупа</returns>
    private ScopeViewModel CreateIdentityScopeViewModel(IdentityResource resource)
    {
        // Локализуем ресурс
        var localized = _localizer.Localize(resource);

        // Создание нового объекта ScopeViewModel 
        return new ScopeViewModel
        {
            // Установка значения идентификационного разрешения в свойство Value 
            Value = resource.Name,

            // Установка отображаемого имени в свойство DisplayName 
            DisplayName = localized.Name,

            // Установка флага выделения разрешения в свойство Emphasize 
            Emphasize = resource.Emphasize,

            // Установка флага обязательности разрешения в свойство Required 
            Required = resource.Required,

            // Установка флага выбранности разрешения в свойство Checked 
            Checked = true,

            // Установка описания разрешения в свойство Description 
            Description = localized.Description
        };
    }

    /// <summary>
    /// Создать область видимости ViewModel
    /// </summary>
    /// <param name="parsedScopeValue"></param>
    /// <param name="apiScope">Область api IdentityServer</param>
    /// <returns>Модель скоупа</returns>
    public ScopeViewModel CreateApiScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope)
    {
        // Локализуем api область
        var localized = _localizer.Localize(apiScope, parsedScopeValue.ParsedParameter);

        // Создание нового объекта ScopeViewModel 
        return new ScopeViewModel
        {
            // Установка значения разрешения доступа к API в свойство Value 
            Value = parsedScopeValue.RawValue,

            // Установка отображаемого имени разрешения доступа к API в свойство DisplayName 
            DisplayName = localized.Name,

            // Установка описания разрешения доступа к API в свойство Description 
            Description = localized.Description,

            // Установка флага выделения разрешения в свойство Emphasize 
            Emphasize = apiScope.Emphasize,

            // Установка флага обязательности разрешения в свойство Required 
            Required = apiScope.Required,

            // Установка флага выбранности разрешения в свойство Checked 
            Checked = true
        };
    }

    /// <summary>
    /// Получить область автономного доступа
    /// </summary>
    /// <returns>Модель скоупа</returns>
    private ScopeViewModel CreateOfflineAccessScopeViewModel()
    {
        // Установка значения константы "OfflineAccess" в переменную name 
        const string name = IdentityServerConstants.StandardScopes.OfflineAccess;

        // Локализуем область offline
        var localized = _localizer.LocalizeOffline();

        // Создание нового объекта ScopeViewModel 
        return new ScopeViewModel
        {
            // Установка значения переменной name в свойство Value 
            Value = name,

            // Установка локализованного отображаемого имени разрешения из ресурсов в свойство DisplayName 
            DisplayName = localized.Name,

            // Установка локализованного описания разрешения из ресурсов в свойство Description 
            Description = localized.Description,

            // Установка флага выделения разрешения в свойство Emphasize 
            Emphasize = true,

            // Установка флага обязательности разрешения в свойство Required 
            Required = false,

            // Установка флага выбранности разрешения в свойство Checked 
            Checked = true
        };
    }
}