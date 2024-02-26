using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PJMS.AuthService.Abstractions.Commands.TwoFactor;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Services.Commands.TwoFactor;

namespace PJMS.AuthService.Tests.Commands.TwoFactor;

/// <summary>
/// Тестовый класс для SetupTwoFactorCommandHandler.
/// </summary>
public class SetupTwoFactorCommandHandlerTest
{
    /// <summary>
    /// Поле Mock объекта UserManager.
    /// </summary>
    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    /// <summary>
    /// Поле обработчика.
    /// </summary>
    private readonly SetupTwoFactorCommandHandler _handler;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SetupTwoFactorCommandHandlerTest()
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
        _handler = new SetupTwoFactorCommandHandler(_userManagerMock.Object);
    }

    /// <summary>
    /// Проверка валидной команды для получения аутентификатора для подключения 2FA.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_Setup()
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

        // Настройка mock объекта UserManager для возвращения false при вызове GetTwoFactorEnabledAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку. 
            .Setup(m => m.GetTwoFactorEnabledAsync(It.IsAny<AppUser>()))

            // Возвращаем false -> 2FA не включена.  
            .ReturnsAsync(() => false);

        // Создаем команду для получения аутентификатора для подключения 2FA и задаем id пользователя.
        var command = new SetupTwoFactorCommand { UserId = Guid.NewGuid() };

        //Act
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
        // Настройка mock объекта UserManager для возвращения null при вызове FindByLoginAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByIdAsync(It.IsAny<string>()))

            // Возвращаем null
            .ReturnsAsync(() => null);

        // Создаем команду для получения аутентификатора для подключения 2FA и задаем id пользователя.
        var command = new SetupTwoFactorCommand { UserId = Guid.NewGuid() };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Проверка случая, когда 2FA уже включена.
    /// </summary>
    [Fact]
    public async Task Handle_WhenTwoFactorEnabled_ThrowsTwoFactorAlreadyEnabledException()
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

        // Настройка mock объекта UserManager для возвращения true при вызове GetTwoFactorEnabledAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку. 
            .Setup(m => m.GetTwoFactorEnabledAsync
                (It.IsAny<AppUser>()))

            // Возвращаем true -> 2FA включена.  
            .ReturnsAsync(() => true);

        // Создаем команду для получения аутентификатора для подключения 2FA и задаем id пользователя.
        var command = new SetupTwoFactorCommand { UserId = Guid.NewGuid() };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<TwoFactorAlreadyEnabledException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}