namespace Overoom.Infrastructure.Storage.Models.Films;

public class VoiceModel
{
    public long Id { get; set; }
    public string Info { get; set; } = null!;
    public CdnModel Cdn { get; set; } = null!;
}