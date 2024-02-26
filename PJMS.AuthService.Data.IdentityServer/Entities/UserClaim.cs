namespace PJMS.AuthService.Data.IdentityServer.Entities;

public abstract class UserClaim
{
    public int Id { get; set; }
    public string Type { get; set; }
}