using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PJMS.AuthService.Abstractions.AppEmailService;
using PJMS.AuthService.Abstractions.Commands.Password;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Services.Commands.Password;

namespace PJMS.AuthService.Tests.Commands.Password;

/// <summary>
/// Тестовый класс для RequestRecoverPasswordCommandHandler.
/// </summary>
public class RequestRecoverPasswordCommandHandlerTest
{
    /// <summary>
    /// Поле Mock объекта UserManager.
    /// </summary>
    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    /// <summary>
    /// Поле Mock объекта EmailService.
    /// </summary>
    private readonly Mock<IEmailService> _emailServiceMock = new();

    /// <summary>
    /// Поле обработчика.
    /// </summary>
    private readonly RequestRecoverPasswordCommandHandler _handler;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RequestRecoverPasswordCommandHandlerTest()
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
        _handler = new  RequestRecoverPasswordCommandHandler(_userManagerMock.Object, _emailServiceMock.Object);
    }
    
    /// <summary>
    /// Проверка валидной команды запроса на восстановление пароля у пользователя.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_SendRequest()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByEmailAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            
            // Возвращаем тестового пользователя.  
            .ReturnsAsync(() => new AppUser
        {
            UserName = "test",
            Email = "test@example.com",
            TimeRegistration = DateTime.UtcNow,
            TimeLastAuth = DateTime.UtcNow,
            Locale = Localization.Az
        });

        // Настройка mock объекта UserManager для возврата true(почта подтверждена) при вызове IsEmailConfirmedAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.IsEmailConfirmedAsync(It.IsAny<AppUser>()))
            
            // Возвращаем true -> почта подтверждена.
            .ReturnsAsync(() => true);
        
        // Создаем команду для запроса восстановления пароля.
        var command = new RequestRecoverPasswordCommand
        {
            // Задаем электронную почту пользователя.
            Email = "test@example.com",
            
            // Задаем url для сброса пароля.
            ResetUrl = "test_url"
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
    /// Проверка случая, когда пользователь не найден по почте.
    /// </summary>
    [Fact]
    public async Task Handle_WhenUserNotFoundByEmail_ThrowsUserNotFoundException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByEmailAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            
            // Возвращаем тестового пользователя.  
            .ReturnsAsync(() => null);
        
        // Создаем команду для запроса восстановления пароля.
        var command = new RequestRecoverPasswordCommand
        {
            // Задаем электронную почту пользователя.
            Email = "test@example.com",
            
            // Задаем url для сброса пароля.
            ResetUrl = "test_url"
        };
        
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
    
    /// <summary>
    /// Проверка случая, когда почта не подтверждена у пользователя.
    /// </summary>
    [Fact]
    public async Task Handle_WhenEmailNotConfirmed_ThrowsEmailNotConfirmedException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения пользователя при вызове FindByEmailAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            
            // Возвращаем тестового пользователя.  
            .ReturnsAsync(() => new AppUser
        {
            UserName = "test",
            Email = "test@example.com",
            TimeRegistration = DateTime.UtcNow,
            TimeLastAuth = DateTime.UtcNow,
            Locale = Localization.Az
        });

        // Настройка mock объекта UserManager для возврата false(почта не подтверждена) при вызове IsEmailConfirmedAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.IsEmailConfirmedAsync(It.IsAny<AppUser>()))
            
            // Возвращаем false -> почта не подтверждена.
            .ReturnsAsync(() => false);
        
        // Создаем команду для запроса восстановления пароля.
        var command = new RequestRecoverPasswordCommand
        {
            // Задаем электронную почту пользователя.
            Email = "test@example.com",
            
            // Задаем url для сброса пароля.
            ResetUrl = "test_url"
        };
        
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения EmailNotConfirmedException.
        await Assert.ThrowsAsync<EmailNotConfirmedException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}