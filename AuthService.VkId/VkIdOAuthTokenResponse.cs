using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AuthService.VkId;

/// <summary> 
/// Класс, содержащий методы расширения для работы с ответом токена аутентификации ВКонтакте. 
/// </summary> 
public static class VkIdOAuthTokenResponse
{
    /// <summary> 
    /// Получает объект <see cref="OAuthTokenResponse"/> из JSON-строки. 
    /// </summary> 
    /// <param name="json">JSON-строка.</param> 
    /// <returns>Объект <see cref="OAuthTokenResponse"/>.</returns> 
    public static OAuthTokenResponse GetOAuthTokenResponse(this string json) 
    { 
        
        // Парсим json
        var body = JsonDocument.Parse(json); 
        
        // Получаем корневой элемент json
        var root = body.RootElement; 
 
        // Проверка наличия свойства "response" в корневом элементе 
        if (!root.TryGetProperty("response", out var content)) 
        { 
            // Если свойство "response" отсутствует, используется свойство "error" 
            content = root.GetProperty("error"); 
        } 
 
        // Создание объекта OAuthTokenResponse с успешным результатом 
        var response = OAuthTokenResponse.Success(body); 
 
        // Установка значения Response объекта response 
        response.Response = JsonDocument.Parse(content.ToString()); 
 
        // Получение значения access_token из свойства content 
        response.AccessToken = content.GetString("access_token"); 
 
        // Получение значения token_type из свойства content 
        response.TokenType = content.GetString("token_type"); 
 
        // Получение значения refresh_token из свойства content 
        response.RefreshToken = content.GetString("refresh_token"); 
 
        // Получение значения expires_in из свойства content 
        response.ExpiresIn = content.GetString("expires_in"); 
 
        // Получение объекта ошибки из свойства content 
        response.Error = GetStandardErrorException(content); 
 
        // Возвращаем ответ
        return response; 
    }

    /// <summary> 
    /// Получает исключение стандартной ошибки из JSON-элемента. 
    /// </summary> 
    /// <param name="response">JSON-элемент с ответом.</param> 
    /// <returns>Исключение стандартной ошибки или null, если ошибка отсутствует.</returns> 
    private static Exception? GetStandardErrorException(JsonElement response) 
    { 
        
        // Получаем ошибку из json
        var error = response.GetString("error_msg"); 
 
        // Проверка наличия ошибки 
        if (error is null) 
        { 
            return null; 
        } 
 
        // Создание строки с сообщением об ошибке 
        var result = new StringBuilder("OAuth token endpoint failure: "); 
        
        // Добавляем в строку ошибку
        result.Append(error); 
 
        // Создание исключения AuthenticationFailureException с сообщением об ошибке 
        var exception = new AuthenticationFailureException(result.ToString()) 
        { 
            Data = 
            { 
                ["error"] = error, 
                ["error_description"] = error 
            } 
        }; 
 
        // Возвращаем исключение
        return exception; 
    }
}