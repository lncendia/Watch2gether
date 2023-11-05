using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.PlaylistManagement.Load;

public class FilmParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Идентификатор фильма")]
    public Guid? Id { get; set; }

    [Display(Name = "Имя")] public string? Name { get; set; }

    [Display(Name = "Описание")] public string? Description { get; set; }

    [Display(Name = "Постер")]
    [DataType(DataType.Url)]
    public string? Uri { get; set; }
}