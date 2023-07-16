using System.ComponentModel.DataAnnotations;
using Overoom.Domain.Films.Enums;

namespace Overoom.WEB.Contracts.Film;

public class GetUriParameters
{
    [Required] public Guid Id { get; set; }
    [Required] public CdnType CdnType { get; set; }
}