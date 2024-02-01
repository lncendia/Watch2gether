using System.ComponentModel.DataAnnotations;
using Films.Domain.Films.Enums;

namespace Films.Infrastructure.Web.FilmsManagement.InputModels;

public class AddFilmInputModel
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(1500, ErrorMessage = "Не больше 1500 символов")]
    [Display(Name = "Описание")]
    public string? Description { get; init; }

    [StringLength(500, ErrorMessage = "Не больше 500 символов")]
    [Display(Name = "Короткое описание")]
    public string? ShortDescription { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Тип")]
    public FilmType? Type { get; init; }

    [Display(Name = "Ссылка на постер")]
    [DataType(DataType.ImageUrl)]
    public string? PosterUrl { get; init; }

    [Display(Name = "Постер")] public string? PosterBase64 { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(200, ErrorMessage = "Не больше 200 символов")]
    [Display(Name = "Название")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Год")]
    [Range(1800, 2100, ErrorMessage = "Введите корректный год выхода")]
    public int? Year { get; init; }

    [Display(Name = "Рейтинг Кинопоиска")]
    [Range(0d, 10d, ErrorMessage = "Рейтинг должен быть в диапазоне от 0 до 10")]
    public double? RatingKp { get; init; }

    [Display(Name = "Рейтинг Imdb")]
    [Range(0d, 10d, ErrorMessage = "Рейтинг должен быть в диапазоне от 0 до 10")]
    public double? RatingImdb { get; init; }

    [Display(Name = "Количество сезонов")] public int? CountSeasons { get; init; }
    [Display(Name = "Количество серий")] public int? CountEpisodes { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Cdn")]
    public CdnInputModel[]? Cdns { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Страны")]
    public TitleInputModel[]? Countries { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Актёры")]
    public ActorInputModel[]? Actors { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Режиссёры")]
    public PersonInputModel[]? Directors { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Жанры")]
    public TitleInputModel[]? Genres { get; init; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Сценаристы")]
    public PersonInputModel[]? Screenwriters { get; init; }
}