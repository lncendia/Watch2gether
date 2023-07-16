using System.ComponentModel.DataAnnotations;
using Overoom.Domain.Films.Enums;

namespace Overoom.WEB.Contracts.FilmManagement;

public class FilmsSearchParameters
{
    [Display(Name = "Название фильма")] public string? Query { get; set; }
    [Required] public int Page { get; set; } = 1;
}