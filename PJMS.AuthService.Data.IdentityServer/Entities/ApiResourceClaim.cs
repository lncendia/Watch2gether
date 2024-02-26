namespace PJMS.AuthService.Data.IdentityServer.Entities;

public class ApiResourceClaim : UserClaim
{
    public int ApiResourceId { get; set; }
    public ApiResource ApiResource { get; set; }
}