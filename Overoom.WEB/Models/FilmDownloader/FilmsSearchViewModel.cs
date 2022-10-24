using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Models.FilmDownloader;

public class FilmsSearchViewModel
{
    [Display(Name = "Название")] public string? Title { get; set; }

    [Display(Name = "Страница")] public int? Page { get; set; }
}