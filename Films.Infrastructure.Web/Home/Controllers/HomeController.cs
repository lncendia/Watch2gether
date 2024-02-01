using System.Diagnostics;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Infrastructure.Web.Home.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Films.Infrastructure.Web.Home.Controllers;

public class HomeController : Controller
{
    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        if (context == null) return Ok();
        var ex = context.Error;
        var text = ex switch
        {
            UserNotFoundException => "Пользователь не найден",
            InvalidCodeException => "Ссылка недействительна",
            UserAlreadyExistException => "Пользователь с таким логином уже существует",
            EmailFormatException => "Неверный формат почты",
            NicknameLengthException => "Длина имени должна составлять от 3 до 20 символов",
            NicknameFormatException =>
                "Имя может содержать только латинские или кириллические буквы, цифры, пробелы и символы подчеркивания",
            PasswordLengthException => "Длина пароля должна составлять от 8 до 30 символов",
            PasswordFormatException =>
                "Пароль должен содержать буквы, цифры и специальные символы и не может иметь разрывов",
            EmailException => "Произошла ошибка при отправке письма",
            FilmNotFoundException => "Фильм не найден",
            ArgumentException => "Некорректные данные",
            ThumbnailSaveException => "Некорректный формат изображения",
            PosterSaveException => "Некорректный формат изображения",
            ViewerInvalidNicknameException => "Неверный формат имени",
            RoomNotFoundException => "Комната не найдена",
            RoomIsFullException => "Комната заполнена",
            UriFormatException => "Неверный формат ссылки",
            InvalidVideoUrlException => "Неверный формат ссылки на видео",
            _ => ex.Message
        };

        return View(new ErrorViewModel(text, Activity.Current?.Id ?? HttpContext.TraceIdentifier));
    }
}