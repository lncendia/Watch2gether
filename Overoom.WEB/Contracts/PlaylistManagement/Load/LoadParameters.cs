using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.PlaylistManagement.Load;

public class LoadParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(200, ErrorMessage = "Не больше 200 символов")]
    [Display(Name = "Название")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(500, ErrorMessage = "Не больше 500 символов")]
    [Display(Name = "Описание")]
    public string? Description { get; set; }

    [Display(Name = "Ссылка на постер")]
    [DataType(DataType.ImageUrl)]
    public string? PosterUri { get; set; }

    [Display(Name = "Постер")]
    [DataType(DataType.Upload)]
    public IFormFile? Poster { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Фильмы")]
    public List<FilmParameters> Films { get; set; } = new();
}