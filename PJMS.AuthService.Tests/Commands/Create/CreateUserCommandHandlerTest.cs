using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Overoom.IntegrationEvents.Users;
using PJMS.AuthService.Abstractions.AppEmailService;
using PJMS.AuthService.Abstractions.Commands.Create;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Services.Commands.Create;

namespace PJMS.AuthService.Tests.Commands.Create;

/// <summary>
/// Тестовый класс для CreateUserCommandHandler.
/// </summary>
public class CreateUserCommandHandlerTests
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
    private readonly CreateUserCommandHandler _handler;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CreateUserCommandHandlerTests()
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
            .Setup(p => p.Publish(It.IsAny<UserCreatedIntegrationEvent>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);
        
        // Инициализация обработчика.
        _handler = new CreateUserCommandHandler(_userManagerMock.Object, _emailServiceMock.Object, publishEndpointMock.Object);
    }
    
    /// <summary>
    /// Проверка валидной команды на создание пользователя.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_CreatesUser()
    {
        // Arrange
        // Настройка mock объекта UserManager для успешного создания пользователя.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            
            // Возвращаем Success.
            .ReturnsAsync(IdentityResult.Success);
        
        // Создаем команду для создания пользователя.
        var command = new CreateUserCommand
        {
            // Задаем почту.
            Email = "test@example.com",
            
            // Задаем пароль.
            Password = "P@$$w0rd1",
            
            // Задаем локализацию пользователя.
            Locale = Localization.Az,
            
            // Задаем Url для подтверждения пользователя.
            ConfirmUrl = "https://example.com/confirm"
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
    /// Проверка случая, когда указан недопустимый email.
    /// </summary>
    [Fact]
    public async Task Handle_WhenEmailIsInvalid_ThrowsEmailFormatException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения ошибки невалидного email.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            
            // Возвращаем ошибку с кодом InvalidEmail.
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "InvalidEmail" }));
        
        // Создаем команду для создания пользователя.
        var command = new CreateUserCommand
        {
            // Задаем почту.
            Email = "test@example.com",
            
            // Задаем пароль.
            Password = "P@$$w0rd1",
            
            // Задаем локализацию пользователя.
            Locale = Localization.Az,
            
            // Задаем Url для подтверждения пользователя.
            ConfirmUrl = "https://example.com/confirm"
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения EmailFormatException.
        await Assert.ThrowsAsync<EmailFormatException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
    
    /// <summary>
    /// Проверка случая, когда email уже используется другим пользователем.
    /// </summary>
    [Fact]
    public async Task Handle_WhenEmailAlreadyTaken_ThrowsEmailAlreadyTakenException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения ошибки дублирования email.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            
            // Возвращаем ошибку с кодом DuplicateEmail.
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail" }));
        
        // Создаем команду для создания пользователя.
        var command = new CreateUserCommand
        {
            // Задаем почту.
            Email = "test@example.com",
            
            // Задаем пароль.
            Password = "P@$$w0rd1",
            
            // Задаем локализацию пользователя.
            Locale = Localization.Az,
            
            // Задаем Url для подтверждения пользователя.
            ConfirmUrl = "https://example.com/confirm"
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения EmailAlreadyTakenException.
        await Assert.ThrowsAsync<EmailAlreadyTakenException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
    
    /// <summary>
    /// Проверка случая, когда пароль не проходит валидацию.
    /// </summary>
    [Fact]
    public async Task Handle_WhenInvalidPassword_ThrowsPasswordValidationException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возврата неудачного результата при вызове CreateAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            
            // Возвращаем результат выполнения метода -> пароль не прошел валидацию.
            .ReturnsAsync(IdentityResult.Failed());

        
        // Создаем команду для создания пользователя.
        var command = new CreateUserCommand
        {
            // Задаем почту.
            Email = "test@example.com",
            
            // Задаем пароль.
            Password = "P@$$w0rd1",
            
            // Задаем локализацию пользователя.
            Locale = Localization.Az,
            
            // Задаем Url для подтверждения пользователя.
            ConfirmUrl = "https://example.com/confirm"
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения PasswordValidationException.
        await Assert.ThrowsAsync<PasswordValidationException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Проверка случая, когда длина имени пользователя не валидна.
    /// </summary>
    [Fact]
    public async Task Handle_WhenUsernameLengthIsInvalid_ThrowsUserNameLengthException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения ошибки дублирования email.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            
            // Возвращаем ошибку с кодом InvalidUserNameLength.
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "InvalidUserNameLength" }));
        
        // Создаем команду для создания пользователя.
        var command = new CreateUserCommand
        {
            // Задаем почту.
            Email = "test@example.com",
            
            // Задаем пароль.
            Password = "P@$$w0rd1",
            
            // Задаем локализацию пользователя.
            Locale = Localization.Az,
            
            // Задаем Url для подтверждения пользователя.
            ConfirmUrl = "https://example.com/confirm"
        };

        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения PasswordValidationException.
        await Assert.ThrowsAsync<UserNameLengthException>(
            () => _handler.Handle(command, CancellationToken.None));

    }
}