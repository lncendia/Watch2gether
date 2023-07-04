using System.ComponentModel.DataAnnotations;
using Overoom.Application.Abstractions.Films.Load.DTOs;
using Overoom.Domain.Films.Enums;

namespace Overoom.WEB.Contracts.FilmLoad;

public class FilmLoadParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(1500, ErrorMessage = "Не больше 1500 символов")]
    [Display(Name = "Описание")]
    public string Description { get; set; } = null!;

    [StringLength(500, ErrorMessage = "Не больше 500 символов")]
    [Display(Name = "Короткое описание")]
    public string? ShortDescription { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Тип")]
    public FilmType Type { get; set; }

    [Display(Name = "Ссылка на постер")]
    [DataType(DataType.Url)]
    public string? PosterUri { get; set; }

    [Display(Name = "Постер")] public IFormFile? Poster { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(200, ErrorMessage = "Не больше 200 символов")]
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;


    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Год")]
    [Range(1800, 2100)]
    public int Year { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Рейтинг")]
    [Range(0, 10)]
    public double RatingKp { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(50, ErrorMessage = "Не больше 50 символов")]
    [Display(Name = "Почта")]
    public int? CountSeasons { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(50, ErrorMessage = "Не больше 50 символов")]
    [Display(Name = "Почта")]
    public int? CountEpisodes { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(50, ErrorMessage = "Не больше 50 символов")]
    [Display(Name = "Почта")]

    public IReadOnlyCollection<CdnDto> CdnList { get; }

    public IReadOnlyCollection<string> Countries { get; }
    public IReadOnlyCollection<(string name, string description)> Actors { get; }
    public IReadOnlyCollection<string> Directors { get; }
    public IReadOnlyCollection<string> Genres { get; }
    public IReadOnlyCollection<string> Screenwriters { get; }
}