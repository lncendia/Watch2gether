using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PJMS.AuthService.Abstractions.Commands.External;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Services.Commands.External;

namespace PJMS.AuthService.Tests.Commands.External;

/// <summary>
/// Тестовый класс для AddUserExternalLoginCommandHandler
/// </summary>
public class AddUserExternalLoginCommandHandlerTest
{
    /// <summary>
    /// Поле Mock объекта UserManager.
    /// </summary>
    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    /// <summary>
    /// Поле обработчика команды.
    /// </summary>
    private readonly AddUserExternalLoginCommandHandler _handler;

    /// <summary>
    /// Поле, представляющее текущего пользователя с его утверждениями (claims).
    /// </summary>
    private readonly ClaimsPrincipal _claimsPrincipal;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AddUserExternalLoginCommandHandlerTest()
    {
        // Инициализация mock объекта UserManager.
        _userManagerMock = new Mock<UserManager<AppUser>>(
            new Mock<IUserStore<AppUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<AppUser>>().Object,
            Array.Empty<IUserValidator<AppUser>>(),
            Array.Empty<IPasswordValidator<AppUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<AppUser>>>().Object);

        // Инициализация обработчика.
        _handler = new AddUserExternalLoginCommandHandler(_userManagerMock.Object);

        // Инициализация ClaimsPrincipal для представления пользователя с указанным email в виде утверждения (claim).
        _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            // Создаем claim с типом Email и введенным значением.
            new Claim(ClaimTypes.Email, "test@example.com")
        }));
    }

    /// <summary>
    /// Проверка на случай, когда все данные валиды.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_AddsExternalLogin()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByIdAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByIdAsync(It.IsAny<string>()))

            // Возвращаем тестового пользователя.
            .ReturnsAsync(() => new AppUser
            {
                UserName = "test",
                Email = "test@example.com",
                TimeRegistration = DateTime.UtcNow,
                TimeLastAuth = DateTime.UtcNow,
                Locale = Localization.Az
            });

        // Настройка mock объекта UserManager для возвращения пустого списка внешних логинов при вызове GetLoginsAsync.
        _userManagerMock

            // Выбираем метод, к которому далем заглушку.
            .Setup(m => m.GetLoginsAsync(It.IsAny<AppUser>()))

            // Возвращаем пустой список внешних логинов.
            .ReturnsAsync(() => new List<UserLoginInfo>());

        // Настройка mock объекта UserManager для успешного добавления внешнего логина при вызове AddLoginAsync.
        _userManagerMock

            // Выбираем метод, к которому далем заглушку.
            .Setup(m => m.AddLoginAsync(It.IsAny<AppUser>(), It.IsAny<ExternalLoginInfo>()))

            // Возвращаем Success.
            .ReturnsAsync(IdentityResult.Success);

        // Создаем команду для добавления внешней аутентификации пользователю.
        var command = new AddUserExternalLoginCommand
        {
            // Id пользователя.
            UserId = Guid.NewGuid(),

            // Задаем информацию о внешней аутентификации.
            LoginInfo = new ExternalLoginInfo(_claimsPrincipal, "TestProvider", "TestKey", "TestDisplayName"),
        };

        // Act
        // Вызов обработчика команды и ожидание возникновения исключения (если такое есть).
        var exception = await Record.ExceptionAsync(async () =>
        {
            await _handler.Handle(command, CancellationToken.None);
        });

        // Assert
        // Проверка на отсутствие исключения.
        Assert.Null(exception);
    }

    /// <summary>
    /// Проверка на случай, когда пользователя не существует.
    /// </summary>
    [Fact]
    public async Task Handle_WhenUserNotFoundById_ThrowsUserNotFoundException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возврата null при вызове FindByIdAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.      
            .Setup(m => m.FindByIdAsync(It.IsAny<string>()))

            // Возвращаем null.
            .ReturnsAsync(() => null);

        // Создаем команду для добавления внешней аутентификации пользователю.
        var command = new AddUserExternalLoginCommand
        {
            // Id пользователя.
            UserId = Guid.NewGuid(),

            // Задаем информацию о внешней аутентификации.
            LoginInfo = new ExternalLoginInfo(_claimsPrincipal, "TestProvider", "TestKey", "TestDisplayName"),
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Проверка на случай, когда провайдер уже существует.
    /// </summary>
    [Fact]
    public async Task Handle_WhenProviderAlreadyExists_ThrowsLoginAlreadyExistsException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возврата пользователя и  при вызове FindByIdAsync 
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByIdAsync(It.IsAny<string>()))

            // Возвращаем тестового пользователя.
            .ReturnsAsync(() => new AppUser
            {
                UserName = "test",
                Email = "test@example.com",
                TimeRegistration = DateTime.UtcNow,
                TimeLastAuth = DateTime.UtcNow,
                Locale = Localization.Az
            });

        // Настройка mock объекта UserManager для возврата списка уже существующих провайдеров.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.GetLoginsAsync(It.IsAny<AppUser>()))

            // Возвращаем тестовый список провайдеров.
            .ReturnsAsync(() => new List<UserLoginInfo>(new[]
            {
                // Создаем тестового провайдера.
                new UserLoginInfo("TestProvider", "TestKey", "TestDisplayName")
            }));

        // Создаем команду для добавления внешней аутентификации пользователю.
        var command = new AddUserExternalLoginCommand
        {
            // Id пользователя.
            UserId = Guid.NewGuid(),

            // Задаем информацию о внешней аутентификации.
            LoginInfo = new ExternalLoginInfo(_claimsPrincipal, "TestProvider", "TestKey", "TestDisplayName"),
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения LoginAlreadyExistsException.
        await Assert.ThrowsAsync<LoginAlreadyExistsException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    ///  Проверка на случай, когда логин уже ассоциирован с пользователем.
    /// </summary>
    [Fact]
    public async Task Handle_WhenLoginAlreadyAssociated_ThrowsLoginAlreadyExistsException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возврата пользователя без ассоциированных логинов и неуспешного добавления логина при вызове AddLoginAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => new AppUser
            {
                UserName = "test",
                Email = "test@example.com",
                TimeRegistration = DateTime.UtcNow,
                TimeLastAuth = DateTime.UtcNow,
                Locale = Localization.Az
            });

        // Настройка mock объекта UserManager 
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.GetLoginsAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(() => new List<UserLoginInfo>());

        // Настройка mock объекта UserManager
        _userManagerMock
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.AddLoginAsync(It.IsAny<AppUser>(), It.IsAny<ExternalLoginInfo>()))
            .ReturnsAsync(IdentityResult.Failed());

        // Создаем команду для добавления внешней аутентификации пользователю.
        var command = new AddUserExternalLoginCommand
        {
            // Id пользователя.
            UserId = Guid.NewGuid(),

            // Задаем информацию о внешней аутентификации.
            LoginInfo = new ExternalLoginInfo(_claimsPrincipal, "TestProvider", "TestKey", "TestDisplayName"),
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения LoginAlreadyAssociatedException.
        await Assert.ThrowsAsync<LoginAlreadyAssociatedException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}