using System.ComponentModel.DataAnnotations;
using Watch2gether.Application.Abstractions.DTO.Films.FilmCatalog;
using Watch2gether.Domain.Films.Enums;

namespace Watch2gether.WEB.Models.Film;

public class FilmsSearchViewModel
{
    [Display(Name = "Название фильма")] public string? Query { get; set; }

    [Display(Name = "Жанр")] public string? Genre { get; set; }
    
    [Display(Name = "Персона")] public string? Person { get; set; }
    [Display(Name = "Страна")] public string? Country { get; set; }
    [Display(Name = "От")] public int? MinYear { get; set; }
    [Display(Name = "До")] public int? MaxYear { get; set; }
    [Display(Name = "Тип")] public FilmType? Type { get; set; }

    [Display(Name = "Сортировать по")]
    [Required]
    public SortBy SortBy { get; set; }

    [Display(Name = "Инверсия сортировки")]
    public bool InverseOrder { get; set; }

    [Display(Name = "Страница")]
    [Required]
    public int Page { get; set; } = 1;
}