﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Abstractions.Queries;
using PJMS.AuthService.Services.Queries;

namespace PJMS.AuthService.Tests.Queries;

/// <summary>
/// Тестовый класс для UserLoginsQueryHandler.
/// </summary>
public class UserLoginsQueryHandlerTest
{
    /// <summary>
    /// Поле Mock объекта UserManager.
    /// </summary>
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    
    /// <summary>
    /// Поле обработчика.
    /// </summary>
    private readonly UserLoginsQueryHandler _handler;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UserLoginsQueryHandlerTest()
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
        _handler = new UserLoginsQueryHandler(_userManagerMock.Object);
    }
    
    /// <summary>
    /// Проверка валидного запроса на получение списка внешних учетных записей пользователя.
    /// </summary>
    [Fact]
    public async Task Handle_ValidQuery_GetLogins()
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
        
        // Настройка mock объекта UserManager для возврата списка уже существующих провайдеров.
        _userManagerMock
                
            // Выбираем метод, к которому делаем заглушку.  
            .Setup(m => m.GetLoginsAsync(It.IsAny<AppUser>()))
            
            // Возвращаем тестовый список провайдеров.
            .ReturnsAsync(() => new List<UserLoginInfo>());
        
        // Создаем запрос для получения списка внешних аутентификаций пользователя и задаем Id пользователя..
        var command = new UserLoginsQuery { Id = Guid.NewGuid() };
        
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
        
        // Создаем запрос для получения списка внешних аутентификаций пользователя и задаем Id пользователя..
        var command = new UserLoginsQuery { Id = Guid.NewGuid() };
        
        // Act & Assert
        // Проверка, что выполнение метода Handle приводит к возникновению исключения UserNotFoundException.
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}