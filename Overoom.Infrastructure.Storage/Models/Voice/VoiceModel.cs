using System.ComponentModel.DataAnnotations.Schema;

namespace Overoom.Infrastructure.Storage.Models.Voice;

public class VoiceModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}