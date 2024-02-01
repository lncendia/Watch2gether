using System.ComponentModel.DataAnnotations;
using Films.Domain.Films.Enums;

namespace Films.Infrastructure.Web.Contracts.Rooms;

public class FilmParameters
{
    [Required] public Guid Id { get; init; }
    [Required] public CdnType CdnType { get; init; }
}