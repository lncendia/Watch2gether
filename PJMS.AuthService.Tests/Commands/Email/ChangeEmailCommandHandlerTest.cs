using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Overoom.IntegrationEvents.Users;
using PJMS.AuthService.Abstractions.Commands.Email;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Services.Commands.Email;

namespace PJMS.AuthService.Tests.Commands.Email;

/// <summary>
/// Тестовый класс для ChangeEmailCommandHandler.
/// </summary>
public class ChangeEmailCommandHandlerTest
{
    /// <summary>
    /// Поле Mock объекта UserManager.
    /// </summary>
    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    /// <summary>
    /// Поле обработчика.
    /// </summary>
    private readonly ChangeEmailCommandHandler _handler;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChangeEmailCommandHandlerTest()
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

        var publishEndpointMock = new Mock<IPublishEndpoint>();

        publishEndpointMock
            .Setup(p => p.Publish(It.IsAny<UserDataChangedIntegrationEvent>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Инициализация обработчика.
        _handler = new ChangeEmailCommandHandler(_userManagerMock.Object, publishEndpointMock.Object);
    }

    /// <summary>
    /// Проверка валидной команды смену эл. почты у пользователя.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_ChangeEmail()
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

        // Настройка mock объекта UserManager для успешной смены почты при вызове ChangeEmailAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.ChangeEmailAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))

            // Возвращаем Success.
            .ReturnsAsync(IdentityResult.Success);

        // Создаем команду для изменения эл. почты у пользователя.
        var command = new ChangeEmailCommand
        {
            // Задаем "тестовый" id пользователя.
            UserId = Guid.NewGuid(),

            // Задаем "тестовый" код подтверждения.
            Code = "test_code",

            // Задаем новую "тестовую" почту.
            NewEmail = "test@example.com"
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
    /// Проверка случая, когда пользователь не найден по id.
    /// </summary>
    [Fact]
    public async Task Handle_WhenUserNotFoundById_ThrowsUserNotFoundException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения null при вызове FindByIdAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByIdAsync(It.IsAny<string>()))

            // Возвращаем null
            .ReturnsAsync(() => null);

        // Создаем команду для изменения эл. почты у пользователя.
        var command = new ChangeEmailCommand
        {
            // Задаем "тестовый" id пользователя.
            UserId = Guid.NewGuid(),

            // Задаем "тестовый" код подтверждения.
            Code = "test_code",

            // Задаем новую "тестовую" почту.
            NewEmail = "test@example.com"
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Проверка случая, когда email уже используется другим пользователем.
    /// </summary>
    [Fact]
    public async Task Handle_WhenEmailAlreadyTaken_ThrowsEmailAlreadyTakenException()
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

        // Настройка mock объекта UserManager для возврата ошибки с кодом при вызове ChangeEmailAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.ChangeEmailAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))

            // Возвращаем ошибку с кодом DuplicateEmail.
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail" }));

        // Создаем команду для изменения эл. почты у пользователя.
        var command = new ChangeEmailCommand
        {
            // Задаем "тестовый" id пользователя.
            UserId = Guid.NewGuid(),

            // Задаем "тестовый" код подтверждения.
            Code = "test_code",

            // Задаем новую "тестовую" почту.
            NewEmail = "test@example.com"
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения EmailAlreadyTakenException.
        await Assert.ThrowsAsync<EmailAlreadyTakenException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Проверка случая, когда пользователь ввел неверный код подтверждения.
    /// </summary>
    [Fact]
    public async Task Handle_WhenInvalidToken_ThrowsInvalidCodeException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByLoginAsync.
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

        // Настройка mock объекта UserManager для возврата ошибки с кодом при вызове ChangeEmailAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.ChangeEmailAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))

            // Возвращаем ошибку с кодом InvalidToken.
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "InvalidToken" }));

        // Создаем команду для изменения эл. почты у пользователя.
        var command = new ChangeEmailCommand
        {
            // Задаем "тестовый" id пользователя.
            UserId = Guid.NewGuid(),

            // Задаем "тестовый" код подтверждения.
            Code = "test_code",

            // Задаем новую "тестовую" почту.
            NewEmail = "test@example.com"
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения InvalidCodeException.
        await Assert.ThrowsAsync<InvalidCodeException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Проверка случая, когда пользователь ввел неверный код подтверждения.
    /// </summary>
    [Fact]
    public async Task Handle_WhenInvalidEmailFormat_ThrowsEmailFormatException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByLoginAsync.
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

        // Настройка mock объекта UserManager для возврата ошибки с кодом при вызове ChangeEmailAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.ChangeEmailAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))

            // Возвращаем ошибку с кодом InvalidToken.
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "InvalidEmail" }));

        // Создаем команду для изменения эл. почты у пользователя.
        var command = new ChangeEmailCommand
        {
            // Задаем "тестовый" id пользователя.
            UserId = Guid.NewGuid(),

            // Задаем "тестовый" код подтверждения.
            Code = "test_code",

            // Задаем новую "тестовую" почту.
            NewEmail = "test@example.com"
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения EmailFormatException.
        await Assert.ThrowsAsync<EmailFormatException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}