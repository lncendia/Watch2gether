using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.PlaylistManagement.InputModels;

public class PlaylistFilmInputModel
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Идентификатор фильма")]
    public Guid? Id { get; init; }
}