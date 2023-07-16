namespace Overoom.WEB.Contracts.Content;

public class SearchParameters
{
    public string? Title { get; set; }
    public string? Person { get; set; }
    public string? Genre { get; set; }
    public string? Country { get; set; }
    public Guid? PlaylistId { get; set; }
}