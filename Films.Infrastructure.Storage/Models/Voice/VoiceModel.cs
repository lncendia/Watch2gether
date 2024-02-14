using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Storage.Models.Voice;

public class VoiceModel
{
    public long Id { get; set; }
    [MaxLength(120)] public string Name { get; set; } = null!;
}