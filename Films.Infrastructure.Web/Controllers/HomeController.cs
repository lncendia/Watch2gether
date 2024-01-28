using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Films.Application.Abstractions.Authentication.Exceptions;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.StartPage.Interfaces;
using Films.Infrastructure.Web.Mappers.Abstractions;
using Films.Infrastructure.Web.Models.Home;
using Films.Domain.Rooms.BaseRoom.Exceptions;
using Films.Domain.Rooms.YoutubeRoom.Exceptions;
using Films.Domain.Users.Exceptions;

namespace Films.Infrastructure.Web.Controllers;

public class HomeController : Controller
{
    private readonly IStartPageService _startPageService;
    private readonly IHomeMapper _homeMapper;

    public HomeController(IStartPageService startPageService, IHomeMapper homeMapper)
    {
        _startPageService = startPageService;
        _homeMapper = homeMapper;
    }

    public async Task<IActionResult> Index(string? message)
    {
        ViewData["Alert"] = message;
        var rooms = _startPageService.GetRoomsAsync();
        var films = _startPageService.GetFilmsAsync();
        var comments = _startPageService.GetCommentsAsync();

        await Task.WhenAll(rooms, films, comments);

        var filmsViewModels = films.Result.Select(_homeMapper.Map).ToList();
        var commentsViewModels = comments.Result.Select(_homeMapper.Map).ToList();
        var roomsViewModels = rooms.Result.Select(_homeMapper.Map).ToList();
        var model = new StartPageViewModel(commentsViewModels, filmsViewModels, roomsViewModels);
        return PartialView(model);
    }

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
            _ => null
        };

        if (text != null) return RedirectToAction("Index", new { message = text });

        return View(new ErrorViewModel(ex.Message, Activity.Current?.Id ?? HttpContext.TraceIdentifier));
    }
}