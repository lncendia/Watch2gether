using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.PlaylistManagement;

public class PlaylistsSearchParameters
{
    [Display(Name = "Название фильма")] public string? Query { get; set; }
    [Required] public int Page { get; set; } = 1;
}