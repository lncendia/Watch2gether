namespace PJMS.AuthService.Data.IdentityServer.Entities;

public class ClientRedirectUri
{
    public int Id { get; set; }
    public string RedirectUri { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }
}