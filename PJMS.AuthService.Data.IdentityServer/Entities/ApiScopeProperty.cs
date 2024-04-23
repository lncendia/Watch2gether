namespace PJMS.AuthService.Data.IdentityServer.Entities;

public class ApiScopeProperty : Property
{
    public int ScopeId { get; set; }
    public ApiScope Scope { get; set; }
}