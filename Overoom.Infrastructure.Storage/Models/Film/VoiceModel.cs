namespace Overoom.Infrastructure.Storage.Models.Film;

public class VoiceModel
{
    public long Id { get; set; }
    public string Info { get; set; } = null!;
    public CdnModel Cdn { get; set; } = null!;
}