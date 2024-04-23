// Импорты не изменились

using System.Security.Claims;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Overoom.IntegrationEvents.Users;
using PJMS.AuthService.Abstractions.AppThumbnailStore;
using PJMS.AuthService.Abstractions.Commands.Create;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Services.Commands.Create;

namespace PJMS.AuthService.Tests.Commands.Create;

/// <summary>
/// Тестовый класс для CreateUserExternalCommandHandler.
/// </summary>
public class CreateUserExternalCommandHandlerTest
{
    /// <summary>
    /// Поле Mock объекта UserManager.
    /// </summary>
    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    /// <summary>
    /// Поле Mock объекта, реализующего IThumbnailStore.
    /// </summary>
    private readonly Mock<IThumbnailStore> _thumbnailStore = new();

    /// <summary>
    /// Поле обработчика.
    /// </summary>
    private readonly CreateUserExternalCommandHandler _handler;

    /// <summary>
    /// Поле, представляющее текущего пользователя с его утверждениями (claims).
    /// </summary>
    private readonly ClaimsPrincipal _claimsPrincipal;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CreateUserExternalCommandHandlerTest()
    {
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
        _handler = new CreateUserExternalCommandHandler(_userManagerMock.Object, _thumbnailStore.Object, publishEndpointMock.Object);

        // Создание ClaimsPrincipal для представления пользователя с указанным email в виде утверждения (claim).
        _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, "test@example.com")
        }));
    }

    /// <summary>
    /// Проверка валидной команды на добавление внешней аутентификации.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_AddExternalLogin()
    {
        // Arrange
        // Настройка mock объекта UserManager для возвращения null при поиске пользователя по внешней аутентификации.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            
            // Возвращаем null.
            .ReturnsAsync(() => null);
            
        // Настройка mock объекта UserManager для успешного создания пользователя.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.CreateAsync(It.IsAny<AppUser>()))
            
            // Возвращаем Success.
            .ReturnsAsync(IdentityResult.Success);
        
        // Создаем команду для создания пользователя через внешний провайдер.
        var command = new CreateUserExternalCommand
        {
            // Задаем "тестовую" информацию о внешней аутентификации.
            LoginInfo = new ExternalLoginInfo(_claimsPrincipal, "TestProvider", "TestKey", "TestDisplayName"), // Здесь укажите правильные значения
            
            // Задаем локализацию пользователя.
            Locale = Localization.Az
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
    /// Проверка случая, когда логин уже ассоциирован с пользователем.
    /// </summary>
    [Fact]
    public async Task Handle_WhenLoginAlreadyAssociated_ThrowsLoginAlreadyAssociatedException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возврата пользователя и  при вызове FindByLoginAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            
            // Возвращаем тестового пользователя.
            .ReturnsAsync(() => 
                new AppUser
        {
            UserName = "test",
            Email = "test@example.com",
            TimeRegistration = DateTime.UtcNow,
            TimeLastAuth = DateTime.UtcNow,
            Locale = Localization.Az
        });
            
        // Создаем команду для создания пользователя через внешний провайдер.
        var command = new CreateUserExternalCommand
        {
            // Задаем "тестовую" информацию о внешней аутентификации.
            LoginInfo = new ExternalLoginInfo(_claimsPrincipal, "TestProvider", "TestKey", "TestDisplayName"), // Здесь укажите правильные значения
            
            // Задаем локализацию пользователя.
            Locale = Localization.Az
        };
            
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения LoginAlreadyAssociatedException.
        await Assert.ThrowsAsync<LoginAlreadyAssociatedException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
        
    /// <summary>
    /// Проверка случая, когда email не существует.
    /// </summary>
    [Fact]
    public async Task Handle_WhenEmailNotExisted_ThrowsEmailFormatException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возврата null при вызове FindByLoginAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            
            // Возвращаем null.
            .ReturnsAsync(() => null);
            
        // Настройка mock объекта UserManager для успешного создания пользователя при вызове CreateAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.CreateAsync(It.IsAny<AppUser>()))
            
            // Возвращаем Success.
            .ReturnsAsync(IdentityResult.Success);

        // Создаем команду для создания пользователя через внешний провайдер.
        var command = new CreateUserExternalCommand
        {
            // Задаем "тестовую" информацию о внешней аутентификации.
            LoginInfo = new ExternalLoginInfo(new ClaimsPrincipal(), "TestProvider", "TestKey", "TestDisplayName"), // Здесь укажите правильные значения
            
            // Задаем локализацию пользователя.
            Locale = Localization.Az
        };
            
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения EmailFormatException.
        await Assert.ThrowsAsync<EmailFormatException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
        
    /// <summary>
    /// Проверка случая, когда email уже занят.
    /// </summary>
    [Fact]
    public async Task Handle_WhenEmailAlreadyTaken_ThrowsEmailAlreadyTakenException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возврата null при вызове FindByLoginAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            
            // Возвращаем null.
            .ReturnsAsync(() => null);
            
        // Настройка mock объекта UserManager для возврата ошибки с кодом DuplicateEmail при вызове CreateAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.CreateAsync(It.IsAny<AppUser>()))
           
            // Возвращаем ошибку с кодом InvalidEmail.
            .ReturnsAsync(IdentityResult.Failed(new IdentityError{ Code = "DuplicateEmail" }));
            
        // Создаем команду для создания пользователя через внешний провайдер.
        var command = new CreateUserExternalCommand
        {
            // Задаем "тестовую" информацию о внешней аутентификации.
            LoginInfo = new ExternalLoginInfo(_claimsPrincipal, "TestProvider", "TestKey", "TestDisplayName"), // Здесь укажите правильные значения
            
            // Задаем локализацию пользователя.
            Locale = Localization.Az
        };
            
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения EmailAlreadyTakenException.
        await Assert.ThrowsAsync<EmailAlreadyTakenException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
        
    /// <summary>
    /// Проверка случая, когда email невалиден.
    /// </summary>
    [Fact]
    public async Task Handle_WhenEmailIsInvalid_ThrowsEmailFormatException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возврата null при вызове FindByLoginAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            
            // Возвращаем null.
            .ReturnsAsync(() => null);
        
        // Настройка mock объекта UserManager для возврата ошибки с кодом InvalidEmail при вызове CreateAsync.
        _userManagerMock
            
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.CreateAsync(It.IsAny<AppUser>()))
            
            // Возвращаем ошибку с кодом InvalidEmail.
            .ReturnsAsync(IdentityResult.Failed(new IdentityError{ Code = "InvalidEmail" }));
            
        // Создаем команду для создания пользователя через внешний провайдер.
        var command = new CreateUserExternalCommand
        {
            // Задаем "тестовую" информацию о внешней аутентификации.
            LoginInfo = new ExternalLoginInfo(_claimsPrincipal, "TestProvider", "TestKey", "TestDisplayName"), // Здесь укажите правильные значения
            
            // Задаем локализацию пользователя.
            Locale = Localization.Az
        };
            
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения EmailFormatException.
        await Assert.ThrowsAsync<EmailFormatException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
    
    /// <summary>
    /// Проверка случая, когда длина имени пользователя не валидна.
    /// </summary>
    [Fact]
    public async Task Handle_WhenUsernameLengthIsInvalid_ThrowsUserNameLengthException()
    {
        // Arrange
        // Настройка mock объекта UserManager для возврата null при вызове FindByLoginAsync.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            
            // Возвращаем null.
            .ReturnsAsync(() => null);
        
        // Настройка mock объекта UserManager для возврата ошибки с кодом InvalidEmail при вызове CreateAsync.
        _userManagerMock
            
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.CreateAsync(It.IsAny<AppUser>()))
            
            // Возвращаем ошибку с кодом InvalidEmail.
            .ReturnsAsync(IdentityResult.Failed(new IdentityError{ Code = "InvalidUserNameLength" }));
            
        // Создаем команду для создания пользователя через внешний провайдер.
        var command = new CreateUserExternalCommand
        {
            // Задаем "тестовую" информацию о внешней аутентификации.
            LoginInfo = new ExternalLoginInfo(_claimsPrincipal, "TestProvider", "TestKey", "TestDisplayName"), // Здесь укажите правильные значения
            
            // Задаем локализацию пользователя.
            Locale = Localization.Az
        };
            
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения EmailFormatException.
        await Assert.ThrowsAsync<UserNameLengthException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}