namespace PJMS.AuthService.Data.IdentityServer.Entities;

public class ApiScopeClaim : UserClaim
{
    public int ScopeId { get; set; }
    public ApiScope Scope { get; set; }
}