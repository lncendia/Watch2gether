namespace PJMS.AuthService.Data.IdentityServer.Entities;

public class IdentityResourceClaim : UserClaim
{
    public int IdentityResourceId { get; set; }
    public IdentityResource IdentityResource { get; set; }
}