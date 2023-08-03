using System.ComponentModel.DataAnnotations;
using Overoom.Domain.Films.Enums;

namespace Overoom.WEB.Contracts.Rooms;

public class FilmParameters
{
    [Required] public Guid Id { get; set; }
    [Required] public CdnType CdnType { get; set; }
}