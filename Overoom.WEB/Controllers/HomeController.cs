using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.Films.Catalog.Exceptions;
using Overoom.Application.Abstractions.Rooms.Exceptions;
using Overoom.Application.Abstractions.StartPage.Interfaces;
using Overoom.Application.Abstractions.Users.Exceptions;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;
using Overoom.Domain.Rooms.YoutubeRoom.Exceptions;
using Overoom.Domain.Users.Exceptions;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models.Home;

namespace Overoom.WEB.Controllers;

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
            InvalidEmailException => "Неверный формат почты",
            InvalidNicknameException => "Неверный формат имени пользователя",
            EmailException => "Произошла ошибка при отправке письма",
            FilmNotFoundException => "Фильм не найден",
            ArgumentException => "Некорректные данные",
            ThumbnailSaveException => "Некорректный формат изображения",
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