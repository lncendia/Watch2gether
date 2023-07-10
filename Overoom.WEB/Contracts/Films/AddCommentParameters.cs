using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.Films;

public class AddCommentParameters
{
    public Guid Id { get; set; }
    [Required]
    [StringLength()]
    public string Text { get; set; }
}