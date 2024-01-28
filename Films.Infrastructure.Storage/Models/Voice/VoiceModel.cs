using System.ComponentModel.DataAnnotations.Schema;

namespace Films.Infrastructure.Storage.Models.Voice;

public class VoiceModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}