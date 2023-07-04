using Microsoft.AspNetCore.Mvc;

namespace Overoom.WEB.Controllers;

public class FilmLoadController : Controller
{
    [HttpGet]
    public IActionResult Load()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Load()
    {
        return View();
    }
    
}