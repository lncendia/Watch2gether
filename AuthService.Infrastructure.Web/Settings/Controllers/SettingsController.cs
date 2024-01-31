using AuthService.Application.Abstractions.Commands;
using AuthService.Application.Abstractions.Entities;
using AuthService.Application.Abstractions.Exceptions;
using AuthService.Application.Abstractions.Queries;
using AuthService.Infrastructure.Web.Exceptions;
using AuthService.Infrastructure.Web.Settings.InputModels;
using AuthService.Infrastructure.Web.Settings.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AuthService.Infrastructure.Web.Settings.Controllers;

/// <summary>
/// Контроллер для изменения настроек аккаунта
/// </summary>
[Authorize]
public class SettingsController : Controller
{
    /// <summary>
    /// Медиатор
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Отвечает за управление поддерживаемыми схемами аутентификации.
    /// </summary>
    private readonly IAuthenticationSchemeProvider _schemeProvider;

    /// <summary>
    /// Предоставляет API для входа пользователя.
    /// </summary>
    private readonly SignInManager<UserData> _signInManager;

    /// <summary>
    /// Локализатор
    /// </summary>
    private readonly IStringLocalizer<SettingsController> _localizer;

    /// <summary>
    /// Конструктор контроллера для прохождения аутентификации.
    /// </summary>
    /// <param name="mediator">Медиатор</param>
    /// <param name="schemeProvider">Отвечает за управление поддерживаемыми схемами
    ///     аутентификации.</param>
    /// <param name="signInManager">Предоставляет API для входа пользователя.</param>
    /// <param name="localizer">Локализатор</param>
    public SettingsController(IMediator mediator, IAuthenticationSchemeProvider schemeProvider,
        SignInManager<UserData> signInManager, IStringLocalizer<SettingsController> localizer)
    {
        _mediator = mediator;
        _schemeProvider = schemeProvider;
        _signInManager = signInManager;
        _localizer = localizer;
    }

    /// <summary>
    /// Страница контроллера по умолчанию
    /// </summary>
    /// <param name="message">Сообщение для пользователя</param>
    /// <param name="returnUrl">URL, на который будет перенаправлен пользователь после завершения аутентификации</param>
    /// <param name="expandElem">Номер вкладки, которая должна быть раскрыта.</param>
    [HttpGet]
    public async Task<IActionResult> Index(string? message, string returnUrl = "/", int expandElem = 1)
    {
        // Получаем аутентифицированного пользователя
        var user = await _mediator.Send(new UserByIdQuery { Id = User.Id() });

        // Создаем модель представления
        var settingsModel = await BuildViewModelAsync(user, returnUrl, expandElem);

        // Устанавливаем сообщение, если оно есть
        if (!string.IsNullOrEmpty(message)) ViewData["message"] = message;

        // Возвращаем представление с моделью настроек
        return View(settingsModel);
    }

    /// <summary>
    /// Метод, который вызывается при перенаправлении на внешний провайдер аутентификации для вызова вызова аутентификации.
    /// </summary>
    /// <param name="provider">Имя внешнего провайдера аутентификации</param>
    /// <param name="returnUrl">URL, на который будет перенаправлен пользователь после завершения аутентификации</param>
    /// <returns>Результат вызова аутентификации</returns>
    [HttpGet]
    public IActionResult Challenge(string? provider, string returnUrl = "/")
    {
        // Проверяем, что имя провайдера не пустое или null
        if (string.IsNullOrEmpty(provider)) throw new QueryParameterMissingException(nameof(provider));

        // Создаем URL для обратного вызова после аутентификации
        var redirectUrl = Url.Action("ExternalLoginCallback", "Settings", new { returnUrl });

        // Конфигурируем свойства аутентификации для внешнего провайдера
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

        // Возвращаем вызов аутентификации с указанным провайдером и свойствами
        return new ChallengeResult(provider, properties);
    }

    /// <summary>
    /// Обрабатывает обратный вызов внешней аутентификации.
    /// </summary>
    /// <param name="returnUrl">URL, на который будет перенаправлен пользователь после завершения аутентификации</param>
    /// <returns>Результат действия IActionResult.</returns>
    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/")
    {
        // Получаем информацию о внешней аутентификации.
        var info = await _signInManager.GetExternalLoginInfoAsync();

        // Если информация отсутствует, вызываем исключение.
        if (info == null) throw new ExternalLoginException();

        // Отправляем команду на добавление внешнего входа и получаем пользователя с обновленными данными
        var user = await _mediator.Send(new AddUserExternalLoginCommand { Id = User.Id(), LoginInfo = info });

        // Отчищаем куки данных от внешнего провайдера
        await HttpContext.SignOutAsync(info.AuthenticationProperties);

        /*Так как Security Stamp у пользователя обновился,
         то переавторизуем его, чтобы обновить куки с новыми данными*/
        await _signInManager.RefreshSignInAsync(user);

        // Перенаправляем пользователя на указанный URL
        return RedirectToAction("Index",
            new { returnUrl, message = InsertWordAfterFirstWord(_localizer["ProviderLinked"], info.LoginProvider) });
    }

    /// <summary>
    /// Метод, который удаляет вход внешнего провайдера аутентификации у пользователя.
    /// </summary>
    /// <param name="provider">Имя внешнего провайдера аутентификации</param>
    /// <param name="returnUrl">URL, на который будет перенаправлен пользователь после удаления входа</param>
    /// <returns>Результат удаления входа</returns>
    public async Task<IActionResult> RemoveLogin(string? provider, string returnUrl = "/")
    {
        // Проверяем, что имя провайдера не пустое или null
        if (string.IsNullOrEmpty(provider)) throw new QueryParameterMissingException(nameof(provider));

        // Отправляем команду на удаление внешнего логина и получаем пользователя с обновленными данными
        var user = await _mediator.Send(
            new RemoveUserExternalLoginCommand { Id = User.Id(), Provider = provider });

        /*Так как Security Stamp у пользователя обновился,
         то переавторизуем его, чтобы обновить куки с новыми данными*/
        await _signInManager.RefreshSignInAsync(user);

        // Перенаправляем пользователя на указанный URL
        return RedirectToAction("Index",
            new { returnUrl, message = InsertWordAfterFirstWord(_localizer["ProviderUnlinked"], provider) });
    }

    /// <summary>
    /// Метод, который изменяет пароль пользователя.
    /// </summary>
    /// <param name="model">Модель с данными для смены пароля</param>
    /// <returns>Результат смены пароля</returns>
    public async Task<IActionResult> ChangePassword(ChangePasswordInputModel model)
    {
        // Объявление переменной message типа string
        string message;

        // Если данные не валидны -> установить сообщение 
        if (!ModelState.IsValid) message = _localizer["CheckCorrectness"];

        // Иначе если данные валидны
        else
        {
            try
            {
                // Отправляем команду на смену пароля и получаем пользователя с обновленными данными
                var user = await _mediator.Send(
                    new ChangePasswordCommand(User.Id(), model.OldPassword, model.NewPassword!));

                // Устанавливаем сообщение "PasswordChanged"
                message = _localizer["PasswordChanged"].ToString();

                /*Так как Security Stamp у пользователя обновился,
                то переавторизуем его, чтобы обновить куки с новыми данными*/
                await _signInManager.RefreshSignInAsync(user);
            }
            catch (Exception ex)
            {
                // Проверяем какое исключение мы словили и устанавливаем соответствующее значение в message
                switch (ex)
                {
                    // В случае если исключение ex является OldPasswordNeededException устанавливаем соответствующее сообщение
                    case OldPasswordNeededException:
                        message = _localizer["OldPasswordNeeded"];
                        break;

                    // В случае если исключение ex является PasswordValidationException устанавливаем соответствующее сообщение
                    case PasswordValidationException passwordValidationException:

                        // Формируем перечисление из локализованных ошибок валидации пароля
                        var errorsEnumerable = passwordValidationException.ValidationErrors
                            .Select(code => _localizer[code.Key]);

                        // Формируем строку из перечисления ошибок через запятую
                        message = string.Join(", ", errorsEnumerable);
                        break;

                    // В случае если исключение ex является ArgumentException устанавливаем соответствующее сообщение
                    case ArgumentException:

                        // Если старый пароль совпадает с новым, то устанавливаем соответствующее сообщение
                        message = _localizer["OldPasswordMatchNew"];
                        break;

                    // В случае если исключение ex является EmailNotConfirmedException устанавливаем соответствующее сообщение
                    case EmailNotConfirmedException:

                        // Если почта не подтверждена, то устанавливаем соответствующее сообщение
                        message = _localizer["EmailNotConfirmed"];
                        break;

                    //Если исключение ex не является ни одним их типов, то вызываем исключение дальше
                    default: throw;
                }
            }
        }

        // Перенаправляем на действие "Index" с указанными параметрами returnUrl, expandElem и message
        return RedirectToAction("Index", new { returnUrl = model.ReturnUrl, expandElem = 2, message });
    }

    /// <summary>
    /// Метод для запроса изменения адреса электронной почты пользователя.
    /// </summary>
    /// <param name="model">Модель ввода, содержащая новый адрес электронной почты и другие соответствующие данные.</param>
    /// <returns>Объект IActionResult, представляющий результат операции.</returns>
    public async Task<IActionResult> RequestChangeEmail(RequestChangeEmailInputModel model)
    {
        // Объявление переменной message типа string
        string message;

        // Присвоение локализованной строки "CheckCorrectness" переменной message, если модель не валидна
        if (!ModelState.IsValid) message = _localizer["CheckCorrectness"];
        else
        {
            // Формирование URL-адреса обратного вызова для изменения адреса электронной почты
            var resetUrl = Url.Action("ChangeEmail", "Settings", new { returnUrl = model.ReturnUrl },
                protocol: HttpContext.Request.Scheme)!;

            try
            {
                await _mediator.Send(new RequestChangeEmailCommand
                {
                    // Идентификатор пользователя
                    Id = User.Id(),

                    // Новая почта
                    NewEmail = model.Email!,

                    // Url для сброса почты и установки новой
                    ResetUrl = resetUrl
                });

                // Присвоение локализованной строки "EmailChangeRequested" переменной message
                message = _localizer["EmailChangeRequested"];
            }
            catch (EmailNotConfirmedException)
            {
                // Если почта не подтверждена, то устанавливаем соответствующее сообщение
                message = _localizer["EmailNotConfirmed"];
            }
        }

        // Перенаправление на действие "Index" с указанными параметрами returnUrl, expandElem и message
        return RedirectToAction("Index", new { returnUrl = model.ReturnUrl, expandElem = 3, message });
    }

    /// <summary>
    /// Метод для изменения адреса электронной почты пользователя.
    /// </summary>
    /// <param name="email">Новый адрес электронной почты.</param>
    /// <param name="code">Код подтверждения изменения адреса электронной почты.</param>
    /// <param name="returnUrl">URL-адрес, на который будет перенаправлен пользователь после изменения адреса электронной почты (по умолчанию "/").</param>
    /// <returns>Объект IActionResult, представляющий результат операции.</returns>
    public async Task<IActionResult> ChangeEmail(string? email, string? code, string returnUrl = "/")
    {
        // Выбрасывание исключения QueryParameterMissingException, если параметр email отсутствует
        if (string.IsNullOrEmpty(email)) throw new QueryParameterMissingException(nameof(email));

        // Выбрасывание исключения QueryParameterMissingException, если параметр code отсутствует
        if (string.IsNullOrEmpty(code)) throw new QueryParameterMissingException(nameof(code));

        // Объявление переменной message типа string
        string message;

        try
        {
            // Отправляем команду на смену почты и получаем пользователя с обновленными данными
            var user = await _mediator.Send(new ChangeEmailCommand
            {
                // Код смены почты
                Code = code,

                // Новая почта
                NewEmail = email,

                // Идентификатор пользователя
                Id = User.Id()
            });

            // Устанавливаем сообщение о том, что почта изменена
            message = _localizer["EmailChanged"];

            /*Так как Security Stamp у пользователя обновился,
            то переавторизуем его, чтобы обновить куки с новыми данными*/
            await _signInManager.RefreshSignInAsync(user);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                //В случае если исключение ex является EmailAlreadyTakenException устанавливаем соответствующее сообщение
                case EmailAlreadyTakenException:
                    message = _localizer["EmailAlreadyTaken"];
                    break;

                //В случае если исключение ex является EmailFormatException устанавливаем соответствующее сообщение
                case EmailFormatException:
                    message = _localizer["EmailFormatInvalid"];
                    break;

                //Если исключение ex не является ни одним их типов, то вызываем исключение дальше
                default: throw;
            }
        }

        // Перенаправление на действие "Index" с указанными параметрами returnUrl, expandElem и message
        return RedirectToAction("Index", new { returnUrl, expandElem = 3, message });
    }

    /// <summary>
    /// Метод для изменения имени пользователя.
    /// </summary>
    /// <param name="model">Модель с данными смены имени.</param>
    /// <returns>Объект IActionResult, представляющий результат операции.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeName(ChangeNameInputModel model)
    {
        // Объявление переменной message типа string
        string message;

        // Присвоение локализованной строки "CheckCorrectness" переменной message, если модель не валидна
        if (!ModelState.IsValid) message = _localizer["CheckCorrectness"];
        else
        {
            try
            {
                var user = await _mediator.Send(new ChangeNameCommand
                {
                    // Идентификатор пользователя
                    Id = User.Id(),

                    // Имя пользователя
                    Name = model.Username!
                });

                // Устанавливаем сообщение о том, что имя изменено
                message = _localizer["UserNameChanged"];

                /*Так как Security Stamp у пользователя обновился,
                то переавторизуем его, чтобы обновить куки с новыми данными*/
                await _signInManager.RefreshSignInAsync(user);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    //В случае если исключение ex является UserNameFormatException добавляем код ошибки в модель
                    case UserNameFormatException:
                        message = _localizer["UserNameFormatInvalid"];
                        break;

                    //В случае если исключение ex является UserNameLengthException добавляем код ошибки в модель
                    case UserNameLengthException:
                        message = _localizer["UserNameLengthInvalid"];
                        break;
                    default: throw;
                }
            }
        }

        return RedirectToAction("Index", new { model.ReturnUrl, expandElem = 4, message });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ChangeAvatar(ChangeAvatarInputModel model)
    {
        // Объявление переменной message типа string
        string message;

        // Присвоение локализованной строки "CheckCorrectness" переменной message, если модель не валидна
        if (!ModelState.IsValid) message = _localizer["CheckCorrectness"];

        // Иначе если размер аватара превышает 15 Мб
        else if (model.File!.Length > 15728640) message = "Размер аватара не может превышать 15 Мб.";

        // Иначе
        else
        {
            // Открываем поток с файлом модели
            await using var stream = model.File!.OpenReadStream();

            // Отправляем команду на смену аватара
            var user = await _mediator.Send(new ChangeAvatarCommand
            {
                // Идентификатор пользователя
                Id = User.Id(),

                // Поток с аватаром
                Avatar = stream
            });

            // Устанавливаем сообщение о том, что почта изменена
            message = _localizer["AvatarChanged"];

            /*Так как Security Stamp у пользователя обновился,
            то переавторизуем его, чтобы обновить куки с новыми данными*/
            await _signInManager.RefreshSignInAsync(user);
        }

        return RedirectToAction("Index", new { model.ReturnUrl, expandElem = 5, message });
    }
    
    /// <summary>
    /// Метод, который заканчивает другие сессии у пользователя
    /// </summary>
    /// <returns>Результат закрытия сессий</returns>
    public async Task<IActionResult> CloseOtherSessions(int expandElem = 1, string returnUrl = "/")
    {
        // Отправляем команду на закрытие всех других сессий, возвращаем данного пользователя
        var user = await _mediator.Send(new CloseOtherUserSessionsCommand { Id = User.Id() });

        // Устанавливаем сообщение "PasswordChanged"
        var message = _localizer["SessionsClosed"];

        /*Так как Security Stamp у пользователя обновился,
        то переавторизуем его, чтобы обновить куки с новыми данными*/
        await _signInManager.RefreshSignInAsync(user);
        
        // Перенаправляем на действие "Index" с указанными параметрами returnUrl, expandElem и message
        return RedirectToAction("Index", new { returnUrl, expandElem = 1, message });
    }

    /// <summary>
    /// Метод, отвечающий за построение модели представления для страницы настроек.
    /// </summary>
    /// <param name="user">Объект пользователя</param>
    /// <param name="returnUrl">URL возврата</param>
    /// <param name="expandElem">Индекс элемента, который нужно раскрыть</param>
    /// <returns>Модель представления настроек</returns>
    private async Task<SettingsViewModel> BuildViewModelAsync(UserData user, string returnUrl, int expandElem)
    {
        // Получаем список входов пользователя
        var logins = await _mediator.Send(new UserLoginsQuery { Id = user.Id });

        // Получаем все схемы аутентификации
        var schemes = await _schemeProvider.GetAllSchemesAsync();

        // Создаем список для хранения внешних провайдеров аутентификации
        var userSchemes = new List<ExternalProvider>();

        // Перебираем все схемы аутентификации для определения доступных провайдеров
        foreach (var authenticationScheme in schemes.Where(scheme => scheme.IsOauthScheme()))
        {
            // Проверяем, связан ли провайдер с пользователем
            var isAssociated = logins.Any(login => login == authenticationScheme.Name);

            // Создаем объект ExternalProvider и добавляем его в список
            userSchemes.Add(new ExternalProvider
            {
                DisplayName = authenticationScheme.DisplayName ?? authenticationScheme.Name,
                AuthenticationScheme = authenticationScheme.Name,
                IsAssociated = isAssociated
            });
        }

        // Создаем модель представления настроек с переданными внешними провайдерами и returnUrl
        var settingsModel = new SettingsViewModel
        {
            ExternalProviders = userSchemes,
            ShowOldPassword = user.PasswordHash != null,
            ExpandElement = expandElem,
            Email = user.Email!,
            ReturnUrl = returnUrl,
            Name = user.UserName!,
            AvatarUrl = user.AvatarUrl
        };

        // Возвращаем модель настроек
        return settingsModel;
    }

    /// <summary>
    /// Вставляет заданное слово после первого слова в исходной строке.
    /// </summary>
    /// <param name="input">Исходная строка</param>
    /// <param name="wordToInsert">Слово для вставки</param>
    /// <returns>Результирующая строка с вставленным словом</returns>
    private static string InsertWordAfterFirstWord(string input, string wordToInsert)
    {
        // Разделяем исходную строку на слова
        var words = input.Split(' ');

        // Проверяем, что в строке есть хотя бы одно слово
        if (words.Length > 0)
        {
            // Вставляем заданное слово после первого слова
            words[0] += " " + wordToInsert;
        }

        // Объединяем слова обратно в строку
        var result = string.Join(" ", words);

        // Возвращаем результат
        return result;
    }
}