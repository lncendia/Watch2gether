using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PJMS.AuthService.Abstractions.Commands.Email;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Services.Commands.Email;

namespace PJMS.AuthService.Tests.Commands.Email;

/// <summary>
/// Тестовый класс для VerifyUserEmailCommandHandler.
/// </summary>
public class VerifyUserEmailCommandHandlerTest
{
    /// <summary>
    /// Поле Mock объекта UserManager.
    /// </summary>
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    
    /// <summary>
    /// Поле обработчика.
    /// </summary>
    private readonly VerifyEmailCommandHandler _handler;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public VerifyUserEmailCommandHandlerTest()
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
        _handler = new VerifyEmailCommandHandler(_userManagerMock.Object);
    }
    
    /// <summary>
    /// Проверка валидной команды подтверждения электронной почты пользователя.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_ConfirmEmail()
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

        // Настройка mock объекта UserManager для успешного подтверждения почты при вызове ConfirmEmailAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.ConfirmEmailAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            
            // Возвращаем Success.
            .ReturnsAsync(IdentityResult.Success);
        
        // Создаем команду для проверки эл. почты пользователя.
        var command = new VerifyEmailCommand
        {
            // Задаем Id пользователя.
            UserId = Guid.NewGuid(),
            
            // Задаем код проверки.
            Code = "test_code"
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
    /// Проверка случая, когда пользователь не найден по Id.
    /// </summary>
    [Fact]
    public async Task Handle_WhenUserNotFoundById_ThrowsUserNotFoundException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения null при вызове FindByIdAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByIdAsync(It.IsAny<string>()))

            // Возвращаем null.  
            .ReturnsAsync(() => null);
        
        // Создаем команду для проверки эл. почты пользователя.
        var command = new VerifyEmailCommand
        {
            // Задаем Id пользователя.
            UserId = Guid.NewGuid(),
            
            // Задаем код проверки.
            Code = "test_code"
        };
        
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
    
    /// <summary>
    /// Проверка случая, когда пользователь ввел неверный код подтверждения.
    /// </summary>
    [Fact]
    public async Task Handle_WhenInvalidCode_ThrowsInvalidCodeException()
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

        // Настройка mock объекта UserManager для неудачного результата при вызове ConfirmEmailAsync.
        _userManagerMock

            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.ConfirmEmailAsync(It.IsAny<AppUser>(), It.IsAny<string>()))

            // Возвращаем результат выполнения метода -> почта не подтверждена.
            .ReturnsAsync(IdentityResult.Failed());
        
        // Создаем команду для проверки эл. почты пользователя.
        var command = new VerifyEmailCommand
        {
            // Задаем Id пользователя.
            UserId = Guid.NewGuid(),
            
            // Задаем код проверки.
            Code = "test_code"
        };
        
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения InvalidCodeException.
        await Assert.ThrowsAsync<InvalidCodeException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}