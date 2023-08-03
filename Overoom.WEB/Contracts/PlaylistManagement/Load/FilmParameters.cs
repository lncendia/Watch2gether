using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.PlaylistManagement.Load;

public class FilmParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Название фильма")]
    public Guid? Id { get; set; }
}