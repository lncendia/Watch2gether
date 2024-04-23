namespace PJMS.AuthService.Data.IdentityServer.Entities;

public class IdentityResourceProperty : Property
{
    public int IdentityResourceId { get; set; }
    public IdentityResource IdentityResource { get; set; }
}