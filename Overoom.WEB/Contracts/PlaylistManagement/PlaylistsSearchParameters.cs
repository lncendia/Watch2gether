using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.PlaylistManagement;

public class PlaylistSearchParameters
{
    [Display(Name = "Название фильма")] public string? Query { get; set; }
    [Required] public int Page { get; set; } = 1;
}