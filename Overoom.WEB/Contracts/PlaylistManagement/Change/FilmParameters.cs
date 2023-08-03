using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.PlaylistManagement.Change;

public class FilmParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Название фильма")]
    public Guid? Id { get; set; }
    
    public string Name { get; set; }
    public string Poster { get; set; }
}