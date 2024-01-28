using AuthService.Application.Abstractions.Queries;
using AuthService.Infrastructure.Web.Exceptions;
using AuthService.Infrastructure.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Infrastructure.Web.Token.Controllers;

[Authorize]
public class TokenController(IMediator mediator, IJwtService jwtService) : Controller
{
    public async Task<IActionResult> GetToken(string? returnUrl)
    {
        if (returnUrl == null || !Uri.TryCreate(returnUrl, UriKind.Absolute, out _))
            throw new QueryParameterMissingException(nameof(returnUrl));

        var user = await mediator.Send(new UserByIdQuery
        {
            Id = User.Id()
        });
        var token = await jwtService.GenerateJwt(user);
        return Redirect(GetReturnUrlWithToken(returnUrl, token));
    }

    private static string GetReturnUrlWithToken(string url, string token)
    {
        // Создаем объект UriBuilder с базовым URL
        var uriBuilder = new UriBuilder(url);

        // Получаем коллекцию параметров запроса
        var queryParameters = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

        // Добавляем параметр "token" со значением
        queryParameters["token"] = token;

        // Устанавливаем обновленную строку запроса
        uriBuilder.Query = queryParameters.ToString();

        // Получаем обновленный URL
        return uriBuilder.ToString();
    }
}