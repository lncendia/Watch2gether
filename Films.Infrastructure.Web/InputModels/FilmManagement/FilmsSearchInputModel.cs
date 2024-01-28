using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Contracts.FilmManagement;

public class FilmsSearchInputModel
{
    [Display(Name = "Название фильма")] public string? Query { get; init; }
    [Required] public int Page { get; init; } = 1;
}