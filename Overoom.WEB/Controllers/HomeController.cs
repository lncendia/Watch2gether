using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.StartPage.Interfaces;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models;
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
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}