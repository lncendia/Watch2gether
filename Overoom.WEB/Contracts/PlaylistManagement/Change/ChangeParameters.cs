using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.PlaylistManagement.Change;

public class ChangeParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public Guid Id { get; set; }

    [StringLength(200, ErrorMessage = "Не больше 200 символов")]
    [Display(Name = "Название")]
    public string? Name { get; set; }

    [StringLength(500, ErrorMessage = "Не больше 500 символов")]
    [Display(Name = "Описание")]
    public string? Description { get; set; }

    [Display(Name = "Ссылка на постер")]
    [DataType(DataType.ImageUrl)]
    public string? NewPosterUri { get; set; }

    [Display(Name = "Постер")]
    [DataType(DataType.Upload)]
    public IFormFile? NewPoster { get; set; }

    public List<FilmParameters> Films { get; set; } = new();
}