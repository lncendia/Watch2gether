using System.ComponentModel.DataAnnotations;

namespace Overoom.Infrastructure.Storage.Models.Abstractions;

public interface IEntityModel
{
    [Key] public long Id { get; set; }
    public int EntityId { get; set; }
}